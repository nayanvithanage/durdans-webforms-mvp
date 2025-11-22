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
    public partial class BookAppointment : System.Web.UI.Page
    {
        private ClinicService _service = new ClinicService();

        // Controls declared in .aspx but missing from designer file
        protected global::System.Web.UI.WebControls.DropDownList ddlPatient;
        protected global::System.Web.UI.WebControls.DropDownList ddlSpecialization;
        protected global::System.Web.UI.WebControls.DropDownList ddlDoctor;
        protected global::System.Web.UI.WebControls.TextBox txtDate;
        protected global::System.Web.UI.WebControls.Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPatients();
            }
        }

        private void LoadPatients()
        {
            ddlPatient.DataSource = _service.GetAllPatients();
            ddlPatient.DataBind();
            ddlPatient.Items.Insert(0, new ListItem("-- Select Patient --", ""));
        }

        protected void ddlSpecialization_SelectedIndexChanged(object sender, EventArgs e)
        {
            string specialization = ddlSpecialization.SelectedValue;
            if (!string.IsNullOrEmpty(specialization))
            {
                var doctors = _service.GetDoctorsBySpecialization(specialization);
                ddlDoctor.DataSource = doctors;
                ddlDoctor.DataBind();
            }
            else
            {
                ddlDoctor.Items.Clear();
            }
            ddlDoctor.Items.Insert(0, new ListItem("-- Select Doctor --", ""));
        }

        protected void Book_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var appointment = new Appointment
                    {
                        PatientId = int.Parse(ddlPatient.SelectedValue),
                        DoctorId = int.Parse(ddlDoctor.SelectedValue),
                        AppointmentDate = DateTime.Parse(txtDate.Text)
                    };

                    _service.BookAppointment(appointment);
                    lblMessage.Text = "Appointment Booked Successfully!";
                    lblMessage.CssClass = "text-success";
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
