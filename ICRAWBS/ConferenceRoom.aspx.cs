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
    public partial class ConferenceRoom : System.Web.UI.Page
    {
        string constring = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

            protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CancellingConferenceBooking();
                if (Session["username"] == null)
                {
                    Response.Redirect("login.aspx");
                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd"));
                }
                else
                {

                    Session["ButtonCSS"] = Session["ButtonCSSPrev"] = "Reserved/Occupied";
                    GetTheDate();
                    txtFrom.Text = txtBookDate.Text = txtTo.Text = Convert.ToDateTime(Session["GetTheDate"].ToString()).ToString("yyyy-MM-dd");
                   
                    txtBookDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    txtBookDateTimeFrom.Text = DateTime.Now.ToLocalTime().ToString("HH:mm");
                    txtBookDateTimeTo.Text = DateTime.Now.ToLocalTime().ToString("HH:mm");
                    Session["dtFrom"] = RoundUp(DateTime.ParseExact(txtBookDateTimeFrom.Text, "HH:mm", null), TimeSpan.FromMinutes(15));
                    Session["dtTo"] = RoundUp(DateTime.ParseExact(txtBookDateTimeTo.Text, "HH:mm", null), TimeSpan.FromMinutes(15));
                    txtBookDate.Attributes["min"] = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                    txtBookDate.Attributes["max"] = DateTime.Now.AddDays(30).ToLocalTime().ToString("yyyy-MM-dd");

                    PopulateddlLocation();
                    PopulatedLocationSelection();
                    PopulatedEmailSelection();

                    if (Session["UserLevel"].ToString() == "Admin")
                    {
                        Session["EmailAddConfirm"] = "All";
                    }
                    else
                    {
                        Session["EmailAddConfirm"] = Session["username"].ToString();
                        EmailAddSelection.Style.Add("display", "none");
                    }
                    Session["LocationSelected"] = "";
                    Session["AddUpdateBooking"] = "add";
                    ButtonCSS();
                }
            }
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
        private void PopulatedEmailSelection()
        {
            EmailAddSelection.Items.Clear();
            EmailAddSelection.Items.Add(new ListItem("All", "All"));
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string sql = "select distinct(emailadd) from bookingmstr where DateTimeFrom between CONVERT(date, @DateTimeFrom) and DATEADD(DAY, 1, CONVERT(date, @DateTimeTo))" +
                "and BookingType = 'Conference Room' and(BookingStatus = 'Reserved' or BookingStatus = 'Occupied') order by emailadd ";
            if (Session["ButtonCSS"].ToString() == "Cancelled")
                sql = "select distinct(emailadd) from bookingmstr where DateTimeFrom between CONVERT(date, @DateTimeFrom) and DATEADD(DAY, 1, CONVERT(date, @DateTimeTo))" +
                "and BookingType = 'Conference Room' and(BookingStatus = 'Cancelled') order by emailadd";
            else if (Session["ButtonCSS"].ToString() == "All")
                sql = "select distinct(emailadd) from bookingmstr where DateTimeFrom between CONVERT(date, @DateTimeFrom) and DATEADD(DAY, 1, CONVERT(date, @DateTimeTo))" +
                "and BookingType = 'Conference Room' and(BookingStatus like '%%') order by emailadd";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@DateTimeFrom", SqlDbType.DateTime).Value = DateTime.Parse(txtFrom.Text);
            cmd.Parameters.Add("@DateTimeTo", SqlDbType.DateTime).Value = DateTime.Parse(txtTo.Text);
            EmailAddSelection.DataSource = cmd.ExecuteReader();
            EmailAddSelection.DataTextField = "emailadd";
            EmailAddSelection.DataBind();
            con.Close();
        }
        protected void lbOccupyCancelUpdate_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            CancellingConferenceBooking();
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            Session["BookIDForUpdate"] = gvRow.Cells[2].Text;
            Session["ButtonAction"] = lb.Text;
            ReadBookingDetails();
            GetTheDate();
            if (Session["Cancelled"].ToString() == "Cancelled" || Session["Cancelled"].ToString() == "Occupied")
            {
                lblWarningMessage.Text = "You can't " + lb.Text.ToLower() + " reservations that are already " + Session["Cancelled"].ToString().ToLower() + ".";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
            }
            else if (Convert.ToDateTime(Session["GetTheDate"].ToString()) < Convert.ToDateTime(Session["ReadBookingFrom"].ToString()) && lb.Text == "Occupy")
            {
                lblWarningMessage.Text = "You can't occupy this reservation. Please wait until " + Convert.ToDateTime(Session["ReadBookingFrom"].ToString()).ToString("MMM dd, yyyy HH:mm tt") + ".";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
            }
            else
            {
                lblMessage.Text = "Are you sure you want to " + lb.Text + " this reservation?";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
            }
        }
        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        private void HideShowButtons()
        {

            lblSaveEmp.Style.Add("display", "none");
            //lblOccupiedConference.Style.Add("display", "none");
            //lbCancelConference.Style.Add("display", "none");
            if (Session["AddUpdateBooking"].ToString() == "add")
            {
                lblSaveEmp.Style.Add("display", "initial");
            }
            //else if (Session["AddUpdateBooking"].ToString() == "update" )
            //{

            //         }
            else if ((Session["UserLevel"].ToString() == "Admin" || Session["EmailAddress"].ToString() == Session["EmailAddConfirm"].ToString()) && (Session["ButtonCSS"].ToString() == "Reserved"|| Session["ButtonCSS"].ToString() == "All") && Session["Cancelled"].ToString()=="Reserved")
            {
                //lbCancelConference.Style.Add("display", "initial");
                //lblOccupiedConference.Style.Add("display", "initial");
            }
            //         else if (Session["AddUpdateBooking"].ToString() == "update" && Session["ButtonCSS"].ToString() == "Booked")
            //{
            //	lblSaveEmp.Style.Add("display", "none");
            //             lbCancelConference.Style.Add("display", "initial");
            //}
        }

        private void ButtonCSS()
        {
            CancellingConferenceBooking();
            if (Session["ButtonCSS"].ToString() == "Reserved/Occupied")
            {
                lbReserved.CssClass = "btn btn-default btn-lg";
                lbCancelled.CssClass = "btn btn-info btn-lg";
                //lbOccupied.CssClass = "btn btn-info btn-lg";
            }
            //else if (Session["ButtonCSS"].ToString() == "Booked")
            //{
            //    lbReserved.CssClass = "btn btn-info btn-lg";
            //    lbCancelled.CssClass = "btn btn-info btn-lg";
            //}
            else if (Session["ButtonCSS"].ToString() == "Cancelled")
            {
                lbReserved.CssClass = "btn btn-info btn-lg";
                lbCancelled.CssClass = "btn btn-default btn-lg";
                //lbOccupied.CssClass = "btn btn-info btn-lg";
            }
            else if (Session["ButtonCSS"].ToString() == "Occupied")
            {
                lbReserved.CssClass = "btn btn-info btn-lg";
                lbCancelled.CssClass = "btn btn-info btn-lg";
                //lbOccupied.CssClass = "btn btn-default btn-lg";
            }
            else if (Session["ButtonCSS"].ToString() == "All")
            {
                lbReserved.CssClass = "btn btn-info btn-lg";
                lbCancelled.CssClass = "btn btn-info btn-lg";
                //lbOccupied.CssClass = "btn btn-info btn-lg";
                Session["ButtonCSSPrev"] = "";
            }
            PopulatedLocationSelection();
            PopulatedEmailSelection();

            if (EmailAddSelection.Items.Contains(new
    ListItem(Session["EmailAddConfirm"].ToString())))
            {
                EmailAddSelection.Text = Session["EmailAddConfirm"].ToString();// ... code here
            }
            else
                Session["EmailAddConfirm"] = "All";
            if (LocationSelection.Items.Contains(new
    ListItem(Session["LocationSelected"].ToString())))
            {
                LocationSelection.Text = Session["LocationSelected"].ToString();
            }
            else
                Session["LocationSelected"] = "";

            gvConferenceRoom.DataSource = ConferenceRoomBookingList();
            gvConferenceRoom.DataBind();
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

        private void PopulatedLocationSelection()
        {
            LocationSelection.Items.Clear();
            LocationSelection.Items.Add(new ListItem("All", ""));
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string username = "";
            if (Session["UserLevel"].ToString() != "Admin")
                username = "and EmailAdd='" + Session["username"].ToString() + "'";
            string sql = "select distinct(Locationname) from bookingmstr where DateTimeFrom between CONVERT(date, @DateTimeFrom) and DATEADD(DAY, 1, CONVERT(date, @DateTimeTo))" +
                "and BookingType = 'Conference Room' and(BookingStatus = 'Reserved' or BookingStatus = 'Occupied') "+username+" order by Locationname";
            if (Session["ButtonCSS"].ToString() == "Cancelled")
                sql = "select distinct(Locationname) from bookingmstr where DateTimeFrom between CONVERT(date, @DateTimeFrom) and DATEADD(DAY, 1, CONVERT(date, @DateTimeTo))" +
                "and BookingType = 'Conference Room' and(BookingStatus = 'Cancelled') " + username + " order by Locationname";
            else if (Session["ButtonCSS"].ToString() == "All")
                sql = "select distinct(Locationname) from bookingmstr where DateTimeFrom between CONVERT(date, @DateTimeFrom) and DATEADD(DAY, 1, CONVERT(date, @DateTimeTo))" +
                "and BookingType = 'Conference Room' and(BookingStatus like '%%') " + username + " order by Locationname";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@DateTimeFrom", SqlDbType.DateTime).Value = DateTime.Parse(txtFrom.Text);
            cmd.Parameters.Add("@DateTimeTo", SqlDbType.DateTime).Value = DateTime.Parse(txtTo.Text);
            LocationSelection.DataSource = cmd.ExecuteReader();
            LocationSelection.DataTextField = "Locationname";
            LocationSelection.DataBind();
            con.Close();
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


        private DataTable ConferenceRoomBookingList()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ReservationRoomBookingList";
            cmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = Session["LocationSelected"].ToString();
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Session["ButtonCSS"].ToString() ;
            cmd.Parameters.Add("@DateTimeFrom", SqlDbType.DateTime).Value = DateTime.Parse(txtFrom.Text);

            cmd.Parameters.Add("@DateTimeTo", SqlDbType.DateTime).Value = DateTime.Parse(txtTo.Text);
            cmd.Parameters.Add("@EmailAdd", SqlDbType.VarChar).Value = Session["EmailAddConfirm"].ToString();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        protected void EmailAddSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["EmailAddConfirm"] = EmailAddSelection.Text;
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
            Session["EmailAddConfirm"] = dr.GetString(5);
            Session["Cancelled"] = dr.GetString(6);
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
            Session["AddUpdateBooking"] = "add";
            Session["BookingStatus"] = "Reserved";
            HideShowButtons();

            txtBookDate.Text = Convert.ToDateTime(Session["GetTheDate"].ToString()).ToString("yyyy-MM-dd");
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
                AddConferenceBooking();
            }
            else if (Session["ButtonAction"].ToString() == "Cancelled"
               || Session["ButtonAction"].ToString() == "Occupied"
               || Session["ButtonAction"].ToString() == "Booked")
            {
                UpdateBooking();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void gvConferenceRoom_RowCreated(object sender, GridViewRowEventArgs e)
        {
            foreach (TableRow row in gvConferenceRoom.Controls[0].Controls)
            {
                row.Cells[2].Visible = false;
                row.Cells[10].Visible = false;
            }

            Control lnkBtnControl = e.Row.Cells[0].FindControl("lbUpdateConferenceRoom");
            if (lnkBtnControl != null)
            {
                Master.ScriptManagerOnMasterPage.RegisterAsyncPostBackControl(lnkBtnControl);
            }
        }

        protected void lbUpdateConferenceRoom_Click(object sender, EventArgs e)
        {
            CancellingConferenceBooking();
            Session["AddUpdateBooking"] = "update";
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            Session["BookIDForUpdate"] = gvRow.Cells[2].Text;
            ReadBookingDetails();
            ddlLocation.Text = Session["ReadBookingLocationName"].ToString();
            txtBookDate.Text = DateTime.Parse(Session["ReadBookingFrom"].ToString()).ToString("yyyy-MM-dd");
            txtBookDateTimeFrom.Text = DateTime.Parse(Session["ReadBookingFrom"].ToString()).ToString("HH:mm");
            txtBookDateTimeTo.Text = DateTime.Parse(Session["ReadBookingTo"].ToString()).ToString("HH:mm");
            txtBookReason.Text = Session["ReadBookingReason"].ToString();
            txtBookRemarks.Text = Session["ReadBookingRemarks"].ToString();
            HideShowButtons();
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewBookingModal();", true);

        }

        protected void lbFilter_Click(object sender, EventArgs e)
        {
            ButtonCSS();
        }

        protected void ButtonCSS_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            Session["ButtonCSS"] = lbtn.Text;
            if (Session["ButtonCSS"]== Session["ButtonCSSPrev"])
                Session["ButtonCSS"] = "All";
            Session["ButtonCSSPrev"] = lbtn.Text;
            ButtonCSS();
        }

        protected void lbCancelConference_Click(object sender, EventArgs e)
        {
            Session["ButtonAction"] = "Cancelled";
            lblMessage.Text = "Are you sure you want to cancel this reservation?";
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
        }
        protected void lbOccupiedConference_Click(object sender, EventArgs e)
        {
            Session["ButtonAction"] = "Occupied";
            lblMessage.Text = "Are you sure you want to occupy this reservation?";
            //if (DateTime.Parse(Session["ReadCurrentDate"].ToString())!=  DateTime.Parse(Session["ReadBookingFrom"].ToString()))
            //{
            //    lblWarningMessage.Text = "Conference Room is not available.";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
            //}
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
        }

        protected void txtFrom_TextChanged(object sender, EventArgs e)
        {
            PopulatedLocationSelection();
            PopulatedEmailSelection();
        }

        protected void LocationSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["LocationSelected"] = LocationSelection.Text;
        }
    }
}