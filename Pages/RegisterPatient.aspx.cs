using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages
{
    public partial class RegisterPatient : System.Web.UI.Page
    {
        private ClinicService _service = new ClinicService();

        // Controls declared in .aspx but missing from designer file
        protected global::System.Web.UI.WebControls.TextBox txtName;
        protected global::System.Web.UI.WebControls.TextBox txtDOB;
        protected global::System.Web.UI.WebControls.TextBox txtContact;
        protected global::System.Web.UI.WebControls.Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var patient = new Patient
                {
                    Name = txtName.Text,
                    DateOfBirth = DateTime.Parse(txtDOB.Text),
                    ContactNumber = txtContact.Text
                };

                int newId = _service.RegisterPatient(patient);
                lblMessage.Text = $"Patient registered successfully! Patient ID: {newId}";
                
                // Clear form
                txtName.Text = "";
                txtDOB.Text = "";
                txtContact.Text = "";
            }
        }
    }
}
