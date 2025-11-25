using System;

namespace Durdans_WebForms_MVP
{
    /// <summary>
    /// Base page class for admin-only pages
    /// Automatically restricts access to users with Admin role
    /// </summary>
    public class AdminBasePage : BasePage
    {
        /// <summary>
        /// Only Admin role is allowed to access pages derived from this class
        /// </summary>
        protected override string[] AllowedRoles => new[] { "Admin" };
    }
}
