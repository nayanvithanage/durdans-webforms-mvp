using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Durdans_WebForms_MVP
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userRole = Session["UserRole"]?.ToString();
            bool isLoggedIn = Session["UserId"] != null;

            // Control navigation item visibility based on role
            // Control navigation item visibility based on role
            if (isLoggedIn)
            {
                string username = Session["Username"]?.ToString();
                
                // Set username labels
                lblUsername.Text = username;
                lblUsernameHeader.Text = username;
                lblSidebarUsername.Text = username;
                lblUserRole.Text = userRole;

                // Show logged-in user sections
                pnlLoggedIn.Visible = true;
                pnlGuest.Visible = false;
                pnlSidebarUser.Visible = true;
                
                // Show Sidebar and Toggle
                pnlSidebar.Visible = true;
                pnlToggleMenu.Visible = true;
                bodyBase.Attributes["class"] = "hold-transition sidebar-mini layout-fixed";
                
                // Show Breadcrumbs for logged in users
                pnlBreadcrumbs.Visible = true;

                if (userRole == "Admin")
                {
                    // Admin can see all menu items
                    liManageDoctors.Visible = true;
                    liManageHospital.Visible = true;
                    liManageSpecializations.Visible = true;
                    liRegisterPatient.Visible = true;
                    liBookAppointment.Visible = true;
                    liDashboard.Visible = true;
                }
                else
                {
                    // Non-admin users (Patient) can only see Book Appointment and Dashboard
                    liManageDoctors.Visible = false;
                    liManageHospital.Visible = false;
                    liManageSpecializations.Visible = false;
                    liRegisterPatient.Visible = false;
                    liBookAppointment.Visible = true;
                    liDashboard.Visible = true;
                }
            }
            else
            {
                // Guest users - hide all navigation items except login/register
                liManageDoctors.Visible = false;
                liManageHospital.Visible = false;
                liManageSpecializations.Visible = false;
                liRegisterPatient.Visible = false;
                liBookAppointment.Visible = false;
                liDashboard.Visible = false;

                // Show guest section
                pnlLoggedIn.Visible = false;
                pnlGuest.Visible = true;
                pnlSidebarUser.Visible = false;
                
                // Hide Sidebar and Toggle, Center Content
                pnlSidebar.Visible = false;
                pnlToggleMenu.Visible = false;
                bodyBase.Attributes["class"] = "hold-transition layout-top-nav";

                // Check if we are on Login or Register page
                string path = Request.AppRelativeCurrentExecutionFilePath.ToLower();
                if (path.Contains("login.aspx") || path.Contains("register.aspx"))
                {
                    // Hide guest nav buttons (Login/Register) on auth pages
                    pnlGuest.Visible = false;
                    
                    // Hide breadcrumbs on auth pages
                    pnlBreadcrumbs.Visible = false;
                }
                else
                {
                    // Show guest nav buttons on other pages (e.g. Home)
                    pnlGuest.Visible = true;
                    
                    // Show breadcrumbs on other pages
                    pnlBreadcrumbs.Visible = true;
                }
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Pages/Login.aspx");
        }
    }
}
