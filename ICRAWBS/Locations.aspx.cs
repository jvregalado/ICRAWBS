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
    public partial class Locations : System.Web.UI.Page
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
					gvLocList.DataSource = LocList();
					gvLocList.DataBind();
				}
			}
		}

		private DataTable LocList()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "LocList";
			cmd.Parameters.Add("@LocName", SqlDbType.VarChar).Value = txtSearch.Text;
			SqlDataAdapter da = new SqlDataAdapter();
			DataTable dt = new DataTable();
			da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			con.Close();
			return dt;
		}

		protected void CheckInputs()
		{
			if(txtLocationName.Text == "" || txtDescription.Text == "" || ddlStatus.Text == "" || ddlType.Text == "")
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
			}
			else
			{
				ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
			}
		}

		private void AddLocation()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "AddLocation";
			cmd.Parameters.Add("@LocationName", SqlDbType.VarChar).Value = txtLocationName.Text;
			cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = txtDescription.Text;
			cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = ddlType.Text;
			cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = ddlStatus.Text;
			cmd.ExecuteNonQuery();
			con.Close();
		}

		private void ReadLocationDetatails()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "ReadLocationDetatails";
			cmd.Parameters.Add("@PK_locationMstr", SqlDbType.VarChar).Value = Session["LocIDForUpdate"].ToString();
			SqlDataReader dr = cmd.ExecuteReader();
			dr.Read();
			Session["ReadLocationName"] = dr.GetString(0);
			Session["ReadLocDesciption"] = dr.GetString(1);
			Session["ReadLoctype"] = dr.GetString(2);
			Session["ReadLocStatus"] = dr.GetString(3);
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			gvLocList.DataSource = LocList();
			gvLocList.DataBind();
		}

		protected void lbAddNewLocation_Click(object sender, EventArgs e)
		{
			Session["AddUpdatLocation"] = "add";
			
			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewLocationModal();", true);
		}

		protected void lblSaveEmp_Click(object sender, EventArgs e)
		{
			AddLocation();
			CheckInputs();
		}

		protected void lblYes_Click(object sender, EventArgs e)
		{

			Response.Redirect(Request.RawUrl);
		}

		protected void gvLocList_RowCreated(object sender, GridViewRowEventArgs e)
		{
			foreach (TableRow row in gvLocList.Controls[0].Controls)
			{
				row.Cells[1].Visible = false;
			}

			Control lnkBtnControl = e.Row.Cells[0].FindControl("lbUpdateLocList");
			if (lnkBtnControl != null)
			{
				Master.ScriptManagerOnMasterPage.RegisterAsyncPostBackControl(lnkBtnControl);
			}
		}

		protected void lbUpdateLocList_Click(object sender, EventArgs e)
		{
			Session["AddUpdatLocation"] = "update";
			GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
			Session["LocIDForUpdate"] = gvRow.Cells[1].Text;
			ReadLocationDetatails();
			txtLocationName.Text = Session["ReadLocationName"].ToString();
			txtDescription.Text = Session["ReadLocDesciption"].ToString();
			ddlType.Text = Session["ReadLoctype"].ToString();
			ddlStatus.Text = Session["ReadLocStatus"].ToString();

			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewLocationModal();", true);
		}
	}
}