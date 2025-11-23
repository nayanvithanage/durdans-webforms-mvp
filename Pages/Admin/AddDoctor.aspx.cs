using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages.Admin
{
    public partial class AddDoctor : System.Web.UI.Page
    {
        private DoctorService _doctorService = new DoctorService();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var doctor = new Doctor
                    {
                        Name = txtName.Text,
                        Specialization = ddlSpecialization.SelectedValue,
                        ConsultationFee = decimal.Parse(txtFee.Text),
                        AvailableDays = txtDays.Text,
                        AvailableTime = txtTime.Text
                    };

                    int newId = _doctorService.RegisterDoctor(doctor);
                    lblMessage.Text = $"Doctor registered successfully! ID: {newId}";
                    lblMessage.CssClass = "text-success";

                    // Clear form
                    txtName.Text = "";
                    ddlSpecialization.SelectedIndex = 0;
                    txtFee.Text = "";
                    txtDays.Text = "";
                    txtTime.Text = "";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger";
                }
            }
        }
    }
}
