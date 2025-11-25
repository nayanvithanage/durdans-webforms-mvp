using System;
using System.Web.UI;

namespace Durdans_WebForms_MVP
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    // User is not logged in - show guest panel
                    pnlGuest.Visible = true;
                    pnlAdminActions.Visible = false;
                    pnlPatientActions.Visible = false;
                    lblWelcome.Text = "Welcome to Durdans Clinic";
                }
                else
                {
                    // User is logged in
                    string username = Session["Username"]?.ToString();
                    string userRole = Session["UserRole"]?.ToString();

                    lblWelcome.Text = $"Welcome, {username}!";

                    // Show/hide panels based on role
                    if (userRole == "Admin")
                    {
                        pnlAdminActions.Visible = true;
                        pnlPatientActions.Visible = false;
                        pnlGuest.Visible = false;
                    }
                    else
                    {
                        pnlAdminActions.Visible = false;
                        pnlPatientActions.Visible = true;
                        pnlGuest.Visible = false;
                    }
                }
            }
        }
    }
}