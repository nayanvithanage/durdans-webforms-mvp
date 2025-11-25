using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages
{
    public partial class RegisterPatient : System.Web.UI.Page
    {
        private PatientService _patientService;

        // Controls declared in .aspx but missing from designer file
        protected global::System.Web.UI.WebControls.TextBox txtName;
        protected global::System.Web.UI.WebControls.TextBox txtDOB;
        protected global::System.Web.UI.WebControls.TextBox txtContact;
        protected global::System.Web.UI.WebControls.Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            _patientService = new PatientService();
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var patient = new Patient
                    {
                        Name = txtName.Text.Trim(),
                        DateOfBirth = DateTime.Parse(txtDOB.Text),
                        ContactNumber = txtContact.Text.Trim()
                    };

                    int newId = _patientService.RegisterPatient(patient);
                    
                    lblMessage.Text = $"Patient registered successfully! Patient ID: {newId}";
                    lblMessage.CssClass = "text-success alert alert-success";

                    // Clear form
                    txtName.Text = "";
                    txtDOB.Text = "";
                    txtContact.Text = "";
                }
                catch (ArgumentException ex)
                {
                    lblMessage.Text = "Validation Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
                catch (InvalidOperationException ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "text-warning alert alert-warning";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An unexpected error occurred: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
                finally
                {
                    _patientService?.Dispose();
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _patientService?.Dispose();
        }
    }
}
