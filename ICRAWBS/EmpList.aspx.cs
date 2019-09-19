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
using System.Net.Mail;

namespace ICRAWBS
{
    public partial class EmpList : System.Web.UI.Page
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
					gvEmpList.DataSource = LoadEmpList();
					gvEmpList.DataBind();
				}
			}
				
        }

		private static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		private DataTable LoadEmpList()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "EmployeeList";
			cmd.Parameters.Add("@EmpName", SqlDbType.VarChar).Value = txtSearch.Text;
			SqlDataAdapter da = new SqlDataAdapter();
			DataTable dt = new DataTable();
			da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			con.Close();
			return dt;
		}

		private void CheckUserExist()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "CheckUserExist";
			cmd.Parameters.Add("@empid", SqlDbType.VarChar).Value = txtlEmployeeID.Text;
			cmd.Parameters.Add("@emailadd", SqlDbType.VarChar).Value = txtEmailAdd.Text;
			SqlDataReader dr = cmd.ExecuteReader();
			dr.Read();
			Session["UserExist"] = dr.GetInt32(0);
			con.Close();
		}

        private void ReadEmpDetails()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "ReadEmpDetails";
			cmd.Parameters.Add("@PK_EmpMstr", SqlDbType.VarChar).Value = Session["UserNameForUpdate"].ToString();
			SqlDataReader dr = cmd.ExecuteReader();
			dr.Read();
			Session["ReadFirstName"] = dr.GetString(0);
			Session["ReadMiddleName"] = dr.GetString(1);
			Session["ReadLastName"] = dr.GetString(2);
			Session["ReadSuffixName"] = dr.GetString(3);
			Session["ReadEmailAdd"] = dr.GetString(4);
			Session["ReadMobileNo"] = dr.GetString(5);
			Session["ReadEmployeeID"] = dr.GetString(6);
			Session["ReadUserlevel"] = dr.GetString(7);
			Session["ReadActive"] = dr.GetBoolean(8);
			Session["ReadConferenceRoomAccess"] = dr.GetBoolean(9);
			Session["ReadWorkStationAccess"] = dr.GetBoolean(10);
			Session["ReadParkingAccess"] = dr.GetBoolean(11);
			con.Close();
		}

		private void CheckInputs()
		{
			if (txtEmailAdd.Text == "" || txtFirstName.Text == "" || txtLastName.Text == "" || txtlEmployeeID.Text == "" || txtMobileNum.Text == "")
			{
				lblWarningMessage.Text = "Please complete required fields.";
				ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
			}
			else
			{
				if (Session["AddUpdatEmployee"].ToString() == "update")
				{
					ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
				}
				else
				{
					CheckUserExist();
					if (Convert.ToInt32(Session["UserExist"].ToString()) > 0)
					{
						lblWarningMessage.Text = "Employee ID or Email Address already exist.";
						ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "WarningModal();", true);
					}
					else
					{
						ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ConfirmModal();", true);
					}
				}
				
			}
		}


		private void AddEmployee()
		{
			SqlConnection con = new SqlConnection(constring);
			con.Open();
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = con;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "AddEmployee";
			cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = txtLastName.Text;
			cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = txtFirstName.Text;
			cmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = txtMiddleName.Text;
			cmd.Parameters.Add("@suffixname", SqlDbType.VarChar).Value = txtSufix.Text;
			cmd.Parameters.Add("@emailadd", SqlDbType.VarChar).Value = txtEmailAdd.Text;
			cmd.Parameters.Add("@mobilenumber", SqlDbType.VarChar).Value = txtMobileNum.Text;
			cmd.Parameters.Add("@EmployeeID", SqlDbType.VarChar).Value = txtlEmployeeID.Text;
			cmd.Parameters.Add("@userlevel", SqlDbType.VarChar).Value = ddlUserLevel.Text;
            if (ckbReset.Checked == true)
            {
                cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = "";
            }
            else
            {
                cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = Base64Encode("logistikus");
            }
            bool SetActiveUser;
			if(ckbActive.Checked == true)
			{
				SetActiveUser = true;
			}
			else
			{
				SetActiveUser = false;
			}
			cmd.Parameters.Add("@Active", SqlDbType.Bit).Value = SetActiveUser;
			
			bool SetConferenceRoomAccess;
			if(ckbConferenceRoom.Checked == true)
			{
				SetConferenceRoomAccess = true;
			}
			else
			{
				SetConferenceRoomAccess = false;
			}
			cmd.Parameters.Add("@ConferenceRoomAccess", SqlDbType.Bit).Value = SetConferenceRoomAccess;

			bool SetWorkStationAccess;
			if(ckbWorkStation.Checked == true)
			{
				SetWorkStationAccess = true;
			}
			else
			{
				SetWorkStationAccess = false;
			}
			cmd.Parameters.Add("@WorkStationAccess", SqlDbType.Bit).Value = SetWorkStationAccess;

			bool SetParkingAccess;
			if (ckbParking.Checked == true)
			{
				SetParkingAccess = true;
			}
			else
			{
				SetParkingAccess = false;
			}
			cmd.Parameters.Add("@ParkingAccess", SqlDbType.Bit).Value = SetParkingAccess;

			cmd.Parameters.Add("@PK_EmpMstr", SqlDbType.VarChar).Value = Session["UserNameForUpdate"].ToString();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		private void CheckUncheck()
		{
			if (Session["ReadActive"].ToString() == "True")
			{
				ckbActive.Checked = true;
			}
			else
			{
				ckbActive.Checked = false;
			}

			if (Session["ReadConferenceRoomAccess"].ToString() == "True")
			{
				ckbConferenceRoom.Checked = true;
			}
			else
			{
				ckbConferenceRoom.Checked = false;
			}

			if (Session["ReadWorkStationAccess"].ToString() == "True")
			{
				ckbWorkStation.Checked = true;
			}
			else
			{
				ckbWorkStation.Checked = false;
			}

			if (Session["ReadParkingAccess"].ToString() == "True")
			{
				ckbParking.Checked = true;
			}
			else
			{
				ckbParking.Checked = false;
			}
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			gvEmpList.DataSource = LoadEmpList();
			gvEmpList.DataBind();
		}

		protected void lbAddNewEmployee_Click(object sender, EventArgs e)
		{
			Session["AddUpdatEmployee"] = "add";
			txtFirstName.Text = "";
			txtLastName.Text = "";
			txtMiddleName.Text = "";
			txtSufix.Text = "";
			txtlEmployeeID.Text = "";
			Session["UserNameForUpdate"] = "";
			txtEmailAdd.Text = "";
			txtMobileNum.Text = "";
			ckbActive.Checked = true;
			ckbConferenceRoom.Checked = false;
			ckbWorkStation.Checked = false;
			ckbParking.Checked = false;

			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewEmployeeModal();", true);
		}

		protected void lblSaveEmp_Click(object sender, EventArgs e)
		{
			CheckInputs();
		}

		protected void lblYes_Click(object sender, EventArgs e)
		{
			AddEmployee();
			Response.Redirect(Request.RawUrl);
		}

		protected void gvEmpList_RowCreated(object sender, GridViewRowEventArgs e)
		{
			foreach (TableRow row in gvEmpList.Controls[0].Controls)
			{
				row.Cells[1].Visible = false;
			}

			Control lnkBtnControl = e.Row.Cells[0].FindControl("lbUpdateEmpList");
			if (lnkBtnControl != null)
			{
				Master.ScriptManagerOnMasterPage.RegisterAsyncPostBackControl(lnkBtnControl);
			}
		}

		protected void lbUpdateEmpList_Click(object sender, EventArgs e)
		{
			Session["AddUpdatEmployee"] = "update";
			GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
			Session["UserNameForUpdate"] = gvRow.Cells[1].Text;
			ReadEmpDetails();

			txtFirstName.Text = Session["ReadFirstName"].ToString();
			txtLastName.Text = Session["ReadLastName"].ToString();
			txtMiddleName.Text = Session["ReadMiddleName"].ToString();
			txtSufix.Text = Session["ReadSuffixName"].ToString();
			txtlEmployeeID.Text = Session["ReadEmployeeID"].ToString();
			txtEmailAdd.Text = Session["ReadEmailAdd"].ToString();
			txtMobileNum.Text = Session["ReadMobileNo"].ToString();
			ddlUserLevel.Text = Session["ReadUserlevel"].ToString();
			CheckUncheck();
			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewEmployeeModal();", true);

		}
	}
}