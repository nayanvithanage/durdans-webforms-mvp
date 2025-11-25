using System;
using System.Web;
using System.Web.UI;
using Durdans_WebForms_MVP.BLL;

namespace Durdans_WebForms_MVP.Pages
{
    public partial class Register : Page
    {
        private UserService _userService;
        private PatientService _patientService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userService = new UserService();
            _patientService = new PatientService();
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    // Create User account (default role is "Patient")
                    var user = new Models.User
                    {
                        Username = txtUsername.Text,
                        Email = txtEmail.Text,
                        Role = "Patient",
                        CreatedDate = DateTime.Now
                    };

                    int userId = _userService.RegisterUser(user.Username, txtPassword.Text, user.Email, user.Role);

                    // Auto-create Patient record for Patient role
                    if (user.Role == "Patient")
                    {
                        var patient = new Models.Patient
                        {
                            Name = txtUsername.Text,
                            DateOfBirth = DateTime.Parse(txtDateOfBirth.Text),
                            ContactNumber = txtContactNumber.Text,
                            UserId = userId
                        };

                        _patientService.RegisterPatient(patient);
                    }

                    // Auto-login after registration
                    Session["UserId"] = userId;
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.Role;

                    Response.Redirect("~/Default.aspx");
                }
                catch (ArgumentException ex)
                {
                    lblMessage.Text = ex.Message;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Registration failed: " + ex.Message;
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _userService?.Dispose();
            _patientService?.Dispose();
        }
    }
}
