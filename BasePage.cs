using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Durdans_WebForms_MVP
{
    /// <summary>
    /// Base page class that provides authentication and authorization functionality
    /// </summary>
    public class BasePage : Page
    {
        /// <summary>
        /// Indicates whether the page requires authentication
        /// Override this in derived classes to allow anonymous access
        /// </summary>
        protected virtual bool RequireAuthentication => true;

        /// <summary>
        /// Specifies which roles are allowed to access this page
        /// null = any authenticated user can access
        /// empty array = no one can access
        /// specific roles = only those roles can access
        /// </summary>
        protected virtual string[] AllowedRoles => null;

        /// <summary>
        /// Called during page initialization to check authorization
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Check authentication requirement
            if (RequireAuthentication)
            {
                if (Session["UserId"] == null)
                {
                    // User is not logged in - redirect to login with return URL
                    string returnUrl = Server.UrlEncode(Request.Url.PathAndQuery);
                    Response.Redirect($"~/Pages/Login.aspx?ReturnUrl={returnUrl}");
                    return;
                }

                // Check role-based authorization
                if (AllowedRoles != null && AllowedRoles.Length > 0)
                {
                    string userRole = Session["UserRole"]?.ToString();

                    if (string.IsNullOrEmpty(userRole) || !AllowedRoles.Contains(userRole))
                    {
                        // User doesn't have required role - redirect to default page
                        Response.Redirect("~/Default.aspx");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current user's ID from session
        /// </summary>
        protected int? CurrentUserId
        {
            get
            {
                if (Session["UserId"] != null)
                {
                    return (int)Session["UserId"];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the current user's username from session
        /// </summary>
        protected string CurrentUsername
        {
            get { return Session["Username"]?.ToString(); }
        }

        /// <summary>
        /// Gets the current user's role from session
        /// </summary>
        protected string CurrentUserRole
        {
            get { return Session["UserRole"]?.ToString(); }
        }

        /// <summary>
        /// Checks if the current user is in the specified role
        /// </summary>
        protected bool IsInRole(string role)
        {
            return CurrentUserRole?.Equals(role, StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Checks if the current user is an admin
        /// </summary>
        protected bool IsAdmin
        {
            get { return IsInRole("Admin"); }
        }
    }
}
