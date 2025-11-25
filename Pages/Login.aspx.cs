using System;
using System.Web;
using System.Web.UI;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages
{
    public partial class Login : Page
    {
        private UserService _userService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userService = new UserService();

            // If user is already logged in, redirect to default page
            if (!IsPostBack && Session["UserId"] != null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var user = _userService.ValidateUser(txtUsername.Text, txtPassword.Text);
                if (user != null)
                {
                    Session["UserId"] = user.Id;
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.Role;

                    // Check for return URL
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        // Validate return URL to prevent open redirect attacks
                        if (IsLocalUrl(returnUrl))
                        {
                            Response.Redirect(returnUrl);
                        }
                        else
                        {
                            Response.Redirect("~/Default.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid username or password.";
                }
            }
        }

        /// <summary>
        /// Validates that a URL is local to prevent open redirect attacks
        /// </summary>
        private bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            // Check if URL is relative
            if (url.StartsWith("/") && !url.StartsWith("//"))
            {
                return true;
            }

            // Check if URL starts with ~/
            if (url.StartsWith("~/"))
            {
                return true;
            }

            return false;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _userService?.Dispose();
        }
    }
}
