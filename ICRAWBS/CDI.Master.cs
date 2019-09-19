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
    public partial class CDI : System.Web.UI.MasterPage
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
                    lblHeader.Text = Session["Header"].ToString();
                    lblUserFullname.Text = Session["UserFullname"].ToString();

                    if (Session["UserLevel"].ToString() == "admin" || Session["UserLevel"].ToString() == "Admin")
                    {
                        liAdministrator.Style.Add("display", "initial");
                        //lblParking.Style.Add("display", "inline");
                        lblWorkStation.Style.Add("display", "none");
                    }
                    else
                    {
                        liAdministrator.Style.Add("display", "none");
                        lblParking.Style.Add("display", "none");
                    }
                }
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        protected void lblIndex_Click(object sender, EventArgs e)
        {
            Session["Header"] = "Reservation Guidelines";
            lblHeader.Text = Session["Header"].ToString();
            Response.Redirect("index.aspx");
        }

        //protected void lbReservations_Click(object sender, EventArgs e)
        //{
        //	Session["Header"] = "Reservations";
        //	lblHeader.Text = Session["Header"].ToString();
        //	Response.Redirect("Reservations.aspx");
        //}

        private void ChangePassword()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_ChangePassword";
            cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session["username"].ToString();
            cmd.Parameters.Add("@NewPassword", SqlDbType.VarChar).Value = Base64Encode(txtNewPassword.Text);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        private void CheckOldPassword()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CheckOldPassword";
            cmd.Parameters.Add("@User", SqlDbType.VarChar).Value = Session["username"].ToString();
            cmd.Parameters.Add("@OldPassword", SqlDbType.VarChar).Value = Base64Encode(txtOldPassword.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            Session["CheckOldPassword"] = dr.GetInt32(0);
            con.Close();
        }

        protected void lblEmpList_Click(object sender, EventArgs e)
        {
            Session["Header"] = "Employee List";
            lblHeader.Text = Session["Header"].ToString();
            Response.Redirect("EmpList.aspx");
        }

        protected void lblLocations_Click(object sender, EventArgs e)
        {
            Session["Header"] = "Locations";
            lblHeader.Text = Session["Header"].ToString();
            Response.Redirect("Locations.aspx");
        }

        protected void lblLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx");
        }




        public ScriptManager ScriptManagerOnMasterPage
        {
            get
            {
                return this.ScriptManager1;
            }

        }

        protected void lblMyReservation_Click(object sender, EventArgs e)
        {
            Session["Header"] = "Conference Reservation";
            lblHeader.Text = Session["Header"].ToString();
            Response.Redirect("MyReservation.aspx");
        }

        protected void lblConferenceRoom_Click(object sender, EventArgs e)
        {
            Session["Header"] = "Conference Room";
            lblHeader.Text = Session["Header"].ToString();
            Response.Redirect("ConferenceRoom.aspx");
        }

        protected void lblWorkStation_Click(object sender, EventArgs e)
        {
            Session["Header"] = "Workstation";
            lblHeader.Text = Session["Header"].ToString();
            Response.Redirect("Workstation.aspx");
        }

        protected void lblParking_Click(object sender, EventArgs e)
        {
            Session["Header"] = "Parking";
            lblHeader.Text = Session["Header"].ToString();
            Response.Redirect("Parking.aspx");
        }


        protected void lblChangePassword_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ChangePasswordModal();", true);
        }
        protected void labelChangePassword_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text == "" || txtOldPassword.Text == "" || txtRePassword.Text == "")
            {
                lblWarningMessageCP.Text = "Please complete required fields.";
            }
            else
            {
                CheckOldPassword();
                if (Convert.ToInt32(Session["CheckOldPassword"].ToString()) != 1)
                {
                    lblWarningMessageCP.Text = "Please enter your current Password.";
                }
                else
                {

                    if (txtNewPassword.Text != txtRePassword.Text)
                    {
                        lblWarningMessageCP.Text = "Password did not match.";
                    }
                    else
                    {
                        ChangePassword();
                        lblWarningMessageCP.Text = "Successfully updated your password.";
                    }

                }
            }
        }
    }
}