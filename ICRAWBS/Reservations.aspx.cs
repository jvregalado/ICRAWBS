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
	public partial class Reservations : System.Web.UI.Page
	{
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

				}
			}
		}

		protected void gvReservationList_RowCreated(object sender, GridViewRowEventArgs e)
		{
			foreach (TableRow row in gvReservationList.Controls[0].Controls)
			{
				row.Cells[1].Visible = false;
			}

			Control lnkBtnControl = e.Row.Cells[0].FindControl("lbUpdateGV");
			if (lnkBtnControl != null)
			{
				Master.ScriptManagerOnMasterPage.RegisterAsyncPostBackControl(lnkBtnControl);
			}
		}

		protected void lbUpdateGV_Click(object sender, EventArgs e)
		{
			Session["AddUpdate"] = "update";
			GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
			Session["IDForUpdate"] = gvRow.Cells[1].Text;
			
			ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "AddNewLocationModal();", true);
		}

		protected void lbConferenceRoom_Click(object sender, EventArgs e)
		{
			lblGVHeader.Text = "Conference Room";
		}

		protected void lbWorkStations_Click(object sender, EventArgs e)
		{
			lblGVHeader.Text = "Work Stations";
		}

		protected void lbParking_Click(object sender, EventArgs e)
		{
			lblGVHeader.Text = "Parking";
		}
	}
}