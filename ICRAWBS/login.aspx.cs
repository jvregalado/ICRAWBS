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
    public partial class login : System.Web.UI.Page
    {
        string constring = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["username"] = null;
             
           
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


        private void UserLogin()
        {
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string sql = "select count(*) from appSysUsers where emailadd=@emailadd AND password=@password";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("emailadd", txtUsername.Text);
            cmd.Parameters.AddWithValue("password", Base64Encode(txtPassword.Text));
            string CheckLogin;
            CheckLogin = cmd.ExecuteScalar().ToString();
            if(Convert.ToInt16(CheckLogin) > 0)
            {
                Session["username"] = txtUsername.Text;
				//Session["UserFullName"] = "Chester Palis";
				ReadUserDetail();
                Session["EmailAddress"]= txtUsername.Text;
				Session["Header"] = "Reservation Guidelines";
                Response.Redirect("index.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "loginModal();", true);
            }
            con.Close();
        }

		private void ReadUserDetail()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "EmployeeFullName";
			cmd.Parameters.Add("@emailadd", SqlDbType.VarChar).Value = txtUsername.Text;
			SqlDataReader dr = cmd.ExecuteReader();
			dr.Read();
			Session["UserFullName"] = dr.GetString(0);
			Session["UserLevel"] = dr.GetString(1);
			con.Close();
		}

        protected void btnLogin_Click(object sender, EventArgs e)
        {
           UserLogin();
        }
    }
}