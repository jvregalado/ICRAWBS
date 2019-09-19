using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ICRAWBS
{
    public partial class MyReservation : System.Web.UI.Page
    {
        string constring = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("login.aspx");
                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd"));
                }
                else
                {
                    GetTheDate();
                    LocationSelection.Items.Add(new ListItem("All", ""));
                    txtFrom.Text = txtBookDate.Text = Convert.ToDateTime(Session["GetTheDate"].ToString()).ToString("yyyy-MM-dd");
                    PopulatedLocationSelection();
                    PopulatedEmailSelection();
                    PopulateddlLocation();
                    if (Session["UserLevel"].ToString() == "Admin")
                    {
                        Session["EmailAddConfirm"] = "All";
                    }
                    else
                    {
                        Session["EmailAddConfirm"] = Session["username"].ToString();
                        EmailAddSelection.Style.Add("display", "none");
                    }
                    gvReservation.DataSource = ReservationRoomBookingList();
                    gvReservation.DataBind();
                }
            }

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
            Session["ReadBookingFrom"] = dr.GetDateTime(1);
            Session["Cancelled"] = dr.GetString(6);
        }
        private void GetTheDate()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string sql = "select getdate()";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            Session["GetTheDate"] = dr.GetDateTime(0);
            con.Close();
        }
        private DataTable ReservationRoomBookingList()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ReservationRoomBookingList";
            cmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = LocationSelection.Text;
            cmd.Parameters.Add("@DateTimeFrom", SqlDbType.DateTime).Value = DateTime.Parse(txtFrom.Text);
            cmd.Parameters.Add("@EmailAdd", SqlDbType.VarChar).Value = Session["EmailAddConfirm"].ToString();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }
        private void PopulatedLocationSelection()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string sql = "select Locationname from locationMstr where Loctype='Conference Room' order by LocationName";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            LocationSelection.DataSource = cmd.ExecuteReader();
            LocationSelection.DataTextField = "Locationname";
            LocationSelection.DataBind();
            con.Close();
        }
        private void PopulatedEmailSelection()
        {
            EmailAddSelection.Items.Clear();
            EmailAddSelection.Items.Add(new ListItem("All", "All"));
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string sql = "select distinct(emailadd) from bookingmstr where DateTimeFrom between CONVERT(date, @DateTimeFrom) and DATEADD(DAY, 1, CONVERT(date, @DateTimeFrom))" +
                "and BookingType = 'Conference Room' and(BookingStatus = 'Reserved' or BookingStatus = 'Occupied') ";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@DateTimeFrom", SqlDbType.DateTime).Value = DateTime.Parse(txtFrom.Text);
            EmailAddSelection.DataSource = cmd.ExecuteReader();
            EmailAddSelection.DataTextField = "emailadd";
            EmailAddSelection.DataBind();
            con.Close();
        }

        private void PopulateddlLocation()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string sql = "select Locationname from locationMstr where Loctype='Conference Room' and locstatus='Available' order by LocationName ";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            ddlLocation.DataSource = cmd.ExecuteReader();
            ddlLocation.DataTextField = "Locationname";
            ddlLocation.DataBind();

            con.Close();
        }

        protected void lblSaveEmp_Click(object sender, EventArgs e)
        {
            CheckInputs();
        }

        private void CheckConferenceBooking()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CheckConferenceBooking";
            cmd.Parameters.Add("@LocationName", SqlDbType.VarChar).Value = ddlLocation.Text;
            cmd.Parameters.Add("@DateTime1", SqlDbType.DateTime).Value = DateTime.Parse(txtBookDate.Text + " " + txtBookDateTimeFrom.Text);
            cmd.Parameters.Add("@DateTime2", SqlDbType.DateTime).Value = DateTime.Parse(txtBookDate.Text + " " + txtBookDateTimeTo.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            Session["CheckConferenceBooking"] = dr.GetInt32(0);
            con.Close();
        }

        private void AddConferenceBooking()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "AddConferenceBooking";
            cmd.Parameters.Add("@EmailAdd", SqlDbType.VarChar).Value = Session["username"].ToString();
            cmd.Parameters.Add("@LocationName", SqlDbType.VarChar).Value = ddlLocation.Text;
            cmd.Parameters.Add("@DateTime1", SqlDbType.DateTime).Value = DateTime.Parse(txtBookDate.Text + " " + txtBookDateTimeFrom.Text);
            cmd.Parameters.Add("@DateTime2", SqlDbType.DateTime).Value = DateTime.Parse(txtBookDate.Text + " " + txtBookDateTimeTo.Text);
            cmd.Parameters.Add("@BookingReason", SqlDbType.VarChar).Value = txtBookReason.Text;
            cmd.Parameters.Add("@BookingStatus", SqlDbType.VarChar).Value = Session["BookingStatus"].ToString();
            cmd.Parameters.Add("@BookingRemarks", SqlDbType.VarChar).Value = txtBookRemarks.Text;
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
                if (Convert.ToDateTime(txtBookDate.Text + " " + txtBookDateTimeFrom.Text) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss")))
                {
                    lblWarningMessage.Text = "Date should not be less than date today.";
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
                        CheckConferenceBooking();
                        if (Convert.ToInt32(Session["CheckConferenceBooking"].ToString()) > 0)
                        {
                            lblWarningMessage.Text = "Conference Room is not available.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
                        }
                        else
                        {
                            Session["ButtonAction"] = "Reserved";
                            lblMessage.Text = "Are you sure you want to save this record?" + txtBookDate.Text;
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
                        }
                    }
                }
            }
        }
        protected void lbBook_Click(object sender, EventArgs e)
        {
            CancellingConferenceBooking();
            Session["ButtonAction"] = "Reserved";
            txtBookDate.Text = Convert.ToDateTime(Session["GetTheDate"].ToString()).ToString("yyyy-MM-dd");

            txtBookDateTimeFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:mm");
            txtBookDateTimeTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:mm");
            txtBookReason.Text = "";
            txtBookRemarks.Text = "";
            ddlLocation.SelectedIndex = 0;
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewBookingModal();", true);
        }
        protected void lbFilter_Click(object sender, EventArgs e)
        {
            gvReservation.DataSource = ReservationRoomBookingList();
            gvReservation.DataBind();
            PopulatedEmailSelection();
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
        private void CancellingConferenceBooking()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CancellingConferenceBooking";
            cmd.ExecuteNonQuery();
            con.Close();
        }
        protected void lblYes_Click(object sender, EventArgs e)
        {
            if (Session["ButtonAction"].ToString() == "Reserve")
                AddConferenceBooking();
                    else
            UpdateBooking();
            Response.Redirect(Request.RawUrl);
        }
            protected void lbOccupyCancelUpdate_Click(object sender, EventArgs e)
        {
            LinkButton lb =(LinkButton)sender;
            CancellingConferenceBooking();
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            Session["BookIDForUpdate"] = gvRow.Cells[2].Text;
            Session["ButtonAction"] = lb.Text;
            ReadBookingDetails();
            if (Session["Cancelled"].ToString() == "Cancelled" || Session["Cancelled"].ToString() == "Occupied")
            {
                lblWarningMessage.Text = "You can't " + lb.Text.ToLower() + " reservation that are already " + Session["Cancelled"].ToString().ToLower() + ".";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
            }
            else if(Convert.ToDateTime(Session["GetTheDate"].ToString())<Convert.ToDateTime(Session["ReadBookingFrom"].ToString())&&lb.Text=="Occupy")
            {
                lblWarningMessage.Text = "You can't occupy this reservation. Please wait until "+ Convert.ToDateTime(Session["GetTheDate"].ToString()) .ToString("MMM dd, yyyy HH:mm tt") + ".";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
            }
            else
            {
                lblMessage.Text = "Are you sure you want to " + lb.Text + " this reservation?";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
            }
        }
            protected void gvReservation_RowCreated(object sender, GridViewRowEventArgs e)
        {
            foreach (TableRow row in gvReservation.Controls[0].Controls)
            {
                row.Cells[2].Visible = false;
                row.Cells[10].Visible = false;
            }

            Control lnkBtnControl = e.Row.Cells[0].FindControl("lbUpdateReservation");
            if (lnkBtnControl != null)
            {
                Master.ScriptManagerOnMasterPage.RegisterAsyncPostBackControl(lnkBtnControl);
            }
        }

        protected void EmailAddSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["EmailAddConfirm"] = EmailAddSelection.Text;
        }

        protected void LocationSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["LocationSelected"]=LocationSelection.Text;
        }
    }
}