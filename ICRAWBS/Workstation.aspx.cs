using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ICRAWBS
{
	public partial class Workstation : System.Web.UI.Page
	{
		string constring = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["username"] == null)
				{
					Response.Redirect("login.aspx");
				}
				else
				{
					txtFrom.Text = DateTime.Today.ToString("yyyy-MM-dd");
					txtTo.Text = DateTime.Today.AddMonths(3).ToString("yyyy-MM-dd");

					//txtFrom.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
					//txtFrom.Attributes["max"] = DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd");

					//txtTo.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
					///txtTo.Attributes["max"] =DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd");

					txtBookDateTimeFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:mm");
					txtBookDateTimeTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:mm");

					txtBookDateTimeFrom.Attributes["min"] = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:mm");
					txtBookDateTimeFrom.Attributes["max"] = DateTime.Now.AddMonths(3).ToLocalTime().ToString("yyyy-MM-ddTHH:mm");

					txtBookDateTimeTo.Attributes["min"] = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:mm");
					txtBookDateTimeTo.Attributes["max"] = DateTime.Now.AddMonths(3).ToLocalTime().ToString("yyyy-MM-ddTHH:mm");

					PopulateddlLocation();
					Session["ButtonCSS"] = "Reserved";
					Session["AddUpdateBooking"] = "add";
					ButtonCSS();
					gvWorkstation.DataSource = WorkstationBookingList();
					gvWorkstation.DataBind();
				}
			}
		}

		private void HideShowButtons()
		{
			if (Session["AddUpdateBooking"].ToString() == "add")
			{
				lblSaveEmp.Style.Add("display", "initial");
				lbBookWorkstation.Style.Add("display", "none");
				lbCancelWorkstation.Style.Add("display", "none");
			}
			else if (Session["AddUpdateBooking"].ToString() == "update" && Session["ButtonCSS"].ToString() == "Reserved")
			{
				lblSaveEmp.Style.Add("display", "none");
				lbBookWorkstation.Style.Add("display", "initial");
				lbCancelWorkstation.Style.Add("display", "initial");
			}
			else if (Session["AddUpdateBooking"].ToString() == "update" && Session["ButtonCSS"].ToString() == "Cancelled")
			{
				lblSaveEmp.Style.Add("display", "none");
				lbBookWorkstation.Style.Add("display", "none");
				lbCancelWorkstation.Style.Add("display", "none");
			}
			else if (Session["AddUpdateBooking"].ToString() == "update" && Session["ButtonCSS"].ToString() == "Booked")
			{
				lblSaveEmp.Style.Add("display", "none");
				lbBookWorkstation.Style.Add("display", "none");
				lbCancelWorkstation.Style.Add("display", "initial");
			}
		}

		private void ButtonCSS()
		{
			if (Session["ButtonCSS"].ToString() == "Reserved")
			{
				lbReserved.CssClass = "btn btn-default btn-lg";
				lbBooked.CssClass = "btn btn-info btn-lg";
				lbCancelled.CssClass = "btn btn-info btn-lg";
			}
			else if (Session["ButtonCSS"].ToString() == "Booked")
			{
				lbReserved.CssClass = "btn btn-info btn-lg";
				lbBooked.CssClass = "btn btn-default btn-lg";
				lbCancelled.CssClass = "btn btn-info btn-lg";
			}
			else if (Session["ButtonCSS"].ToString() == "Cancelled")
			{
				lbReserved.CssClass = "btn btn-info btn-lg";
				lbBooked.CssClass = "btn btn-info btn-lg";
				lbCancelled.CssClass = "btn btn-default btn-lg";
			}
		}

		private void PopulateddlLocation()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			string sql = "select Locationname from locationMstr where Loctype='Workstation' order by LocationName";
			SqlCommand cmd = new SqlCommand(sql, con);
			cmd.CommandType = CommandType.Text;
			ddlLocation.DataSource = cmd.ExecuteReader();
			ddlLocation.DataTextField = "Locationname";
			ddlLocation.DataBind();
			con.Close();
		}

		private void CheckWorkstationBooking()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "CheckWorkstationBooking";
			cmd.Parameters.Add("@LocationName", SqlDbType.VarChar).Value = ddlLocation.Text;
			cmd.Parameters.Add("@DateTime1", SqlDbType.DateTime).Value = DateTime.Parse(txtBookDateTimeFrom.Text);
			cmd.Parameters.Add("@DateTime2", SqlDbType.DateTime).Value = DateTime.Parse(txtBookDateTimeTo.Text);
			SqlDataReader dr = cmd.ExecuteReader();
			dr.Read();
			Session["CheckWorkstationBooking"] = dr.GetInt32(0);
			con.Close();
		}

		private void AddWorkstationBooking()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "AddWorkstationBooking";
			cmd.Parameters.Add("@EmailAdd", SqlDbType.VarChar).Value = Session["username"].ToString();
			cmd.Parameters.Add("@LocationName", SqlDbType.VarChar).Value = ddlLocation.Text;
			cmd.Parameters.Add("@DateTime1", SqlDbType.DateTime).Value = DateTime.Parse(txtBookDateTimeFrom.Text);
			cmd.Parameters.Add("@DateTime2", SqlDbType.DateTime).Value = DateTime.Parse(txtBookDateTimeTo.Text);
			cmd.Parameters.Add("@BookingReason", SqlDbType.VarChar).Value = txtBookReason.Text;
			cmd.Parameters.Add("@BookingStatus", SqlDbType.VarChar).Value = Session["BookingStatus"].ToString();
			cmd.Parameters.Add("@BookingRemarks", SqlDbType.VarChar).Value = txtBookRemarks.Text;
			cmd.ExecuteNonQuery();
			con.Close();
		}

		private DataTable WorkstationBookingList()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "WorkstationBookingList";
			cmd.Parameters.Add("@DateTime1", SqlDbType.DateTime).Value = DateTime.Parse(txtFrom.Text);
			cmd.Parameters.Add("@DateTime2", SqlDbType.DateTime).Value = DateTime.Parse(txtTo.Text + " 23:59:59");
			cmd.Parameters.Add("@BookView", SqlDbType.VarChar).Value = Session["ButtonCSS"].ToString();
			SqlDataAdapter da = new SqlDataAdapter();
			DataTable dt = new DataTable();
			da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			con.Close();
			return dt;
		}

		private void ReadBookingDetails()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "ReadBookingDetails";
			cmd.Parameters.Add("@PK_bookingMstr", SqlDbType.VarChar).Value = Session["BookIDForUpdate"].ToString();
			SqlDataReader dr = cmd.ExecuteReader();
			dr.Read();
			Session["ReadBookingLocationName"] = dr.GetString(0);
			Session["ReadBookingFrom"] = dr.GetDateTime(1);
			Session["ReadBookingTo"] = dr.GetDateTime(2);
			Session["ReadBookingReason"] = dr.GetString(3);
			Session["ReadBookingRemarks"] = dr.GetString(4);
		}

		private void UpdateBooking()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "UpdateBooking";
			cmd.Parameters.Add("@PK_bookingMstr", SqlDbType.VarChar).Value = Session["BookIDForUpdate"].ToString();
			cmd.Parameters.Add("@StatusUpdate", SqlDbType.VarChar).Value = Session["ButtonAction"].ToString();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		protected void CheckInputs()
		{
			if (txtBookDateTimeFrom.Text == "" || txtBookDateTimeTo.Text == "" || txtBookReason.Text == "" || txtBookRemarks.Text == "" || ddlLocation.Text == "")
			{
				lblWarningMessage.Text = "Please complete required fields.";
				ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
			}
			else
			{
				if (Convert.ToDateTime(txtBookDateTimeFrom.Text) > Convert.ToDateTime(txtBookDateTimeTo.Text))
				{
					lblWarningMessage.Text = "\"From Date Time\" cannot be less than to \"To Date Time\"";
					ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
				}
				else
				{
                    CheckWorkstationBooking();
					if (Convert.ToInt32(Session["CheckWorkstationBooking"].ToString()) >= 25)
					{
						lblWarningMessage.Text = "Workstation Room is not available.";
						ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
					}
					else
					{
						Session["ButtonAction"] = "Reserved";
						lblMessage.Text = "Are you sure you want to save this record?";
						ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
					}
				}
			}
		}

		protected void lbBook_Click(object sender, EventArgs e)
		{
			Session["AddUpdateBooking"] = "add";
			Session["BookingStatus"] = "Reserved";
			HideShowButtons();
			txtBookDateTimeFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:mm");
			txtBookDateTimeTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:mm");
			txtBookReason.Text = "";
			txtBookRemarks.Text = "";
			ddlLocation.SelectedIndex = 0;

			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewBookingModal();", true);
		}

		protected void lblSaveEmp_Click(object sender, EventArgs e)
		{
			CheckInputs();
		}

		protected void lblYes_Click(object sender, EventArgs e)
		{
			if (Session["ButtonAction"].ToString() == "Reserved")
			{
				AddWorkstationBooking();
			}
			else if (Session["ButtonAction"].ToString() == "Cancelled")
			{
				UpdateBooking();
			}
			else if (Session["ButtonAction"].ToString() == "Booked")
			{
				UpdateBooking();
			}

			Response.Redirect(Request.RawUrl);
		}

		protected void gvWorkstation_RowCreated(object sender, GridViewRowEventArgs e)
		{
			foreach (TableRow row in gvWorkstation.Controls[0].Controls)
			{
				row.Cells[1].Visible = false;
			}

			Control lnkBtnControl = e.Row.Cells[0].FindControl("lbUpdateWorkstation");
			if (lnkBtnControl != null)
			{
				Master.ScriptManagerOnMasterPage.RegisterAsyncPostBackControl(lnkBtnControl);
			}
		}

		protected void lbUpdateWorkstation_Click(object sender, EventArgs e)
		{
			Session["AddUpdateBooking"] = "update";
			GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
			Session["BookIDForUpdate"] = gvRow.Cells[1].Text;
			ReadBookingDetails();
			ddlLocation.Text = Session["ReadBookingLocationName"].ToString();
			txtBookDateTimeFrom.Text = DateTime.Parse(Session["ReadBookingFrom"].ToString()).ToString("yyyy-MM-ddTHH:mm");
			txtBookDateTimeTo.Text = DateTime.Parse(Session["ReadBookingTo"].ToString()).ToString("yyyy-MM-ddTHH:mm");
			txtBookReason.Text = Session["ReadBookingReason"].ToString();
			txtBookRemarks.Text = Session["ReadBookingRemarks"].ToString();
			HideShowButtons();
			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewBookingModal();", true);

		}

		protected void lbFilter_Click(object sender, EventArgs e)
		{

			gvWorkstation.DataSource = WorkstationBookingList();
			gvWorkstation.DataBind();
		}

		protected void lbReserved_Click(object sender, EventArgs e)
		{
			Session["ButtonCSS"] = "Reserved";
			ButtonCSS();
			gvWorkstation.DataSource = WorkstationBookingList();
			gvWorkstation.DataBind();
		}

		protected void lbBooked_Click(object sender, EventArgs e)
		{
			Session["ButtonCSS"] = "Booked";
			Session["AddUpdateBooking"] = "add";
			ButtonCSS();
			gvWorkstation.DataSource = WorkstationBookingList();
			gvWorkstation.DataBind();
		}

		protected void lbCancelled_Click(object sender, EventArgs e)
		{
			Session["ButtonCSS"] = "Cancelled";
			ButtonCSS();
			gvWorkstation.DataSource = WorkstationBookingList();
			gvWorkstation.DataBind();
		}

		protected void lbCancelWorkstation_Click(object sender, EventArgs e)
		{
			Session["ButtonAction"] = "Cancelled";
			lblMessage.Text = "Are you sure you want to cancel this reservation?";
			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
		}

		protected void lbBookWorkstation_Click(object sender, EventArgs e)
		{
			Session["ButtonAction"] = "Booked";
			lblMessage.Text = "Are you sure you want to book this reservation?";
			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
		}
	}
}