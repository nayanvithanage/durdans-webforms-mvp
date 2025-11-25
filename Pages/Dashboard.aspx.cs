using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages
{
    public partial class Dashboard : BasePage
    {
        private AppointmentService _appointmentService;
        private PatientService _patientService;
        private DoctorService _doctorService;
        private HospitalService _hospitalService;
        private bool _showUpcoming = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            _appointmentService = new AppointmentService();
            _patientService = new PatientService();
            _doctorService = new DoctorService();
            _hospitalService = new HospitalService();

            if (!IsPostBack)
            {
                // Configure UI based on user role
                if (IsAdmin)
                {
                    pnlAdminFilters.Visible = true;
                    LoadFilterDropdowns();
                }
                else
                {
                    pnlAdminFilters.Visible = false;
                }

                LoadAppointments();
            }
        }

        // Add these properties at the top of the class (around line 17)
        private int CurrentPageUpcoming
        {
            get
            {
                if (ViewState["CurrentPageUpcoming"] == null)
                    return 1;
                return (int)ViewState["CurrentPageUpcoming"];
            }
            set { ViewState["CurrentPageUpcoming"] = value; }
        }

        private int CurrentPagePast
        {
            get
            {
                if (ViewState["CurrentPagePast"] == null)
                    return 1;
                return (int)ViewState["CurrentPagePast"];
            }
            set { ViewState["CurrentPagePast"] = value; }
        }

        private int PageSizeUpcoming
        {
            get
            {
                if (ViewState["PageSizeUpcoming"] == null)
                    return 10;
                return (int)ViewState["PageSizeUpcoming"];
            }
            set { ViewState["PageSizeUpcoming"] = value; }
        }

        private int PageSizePast
        {
            get
            {
                if (ViewState["PageSizePast"] == null)
                    return 10;
                return (int)ViewState["PageSizePast"];
            }
            set { ViewState["PageSizePast"] = value; }
        }

        private void LoadFilterDropdowns()
        {
            // Load doctors for filter
            var doctors = _doctorService.GetAllDoctors();
            ddlDoctorFilter.DataSource = doctors;
            ddlDoctorFilter.DataTextField = "Name";
            ddlDoctorFilter.DataValueField = "Id";
            ddlDoctorFilter.DataBind();
            ddlDoctorFilter.Items.Insert(0, new ListItem("All Doctors", ""));

            // Load hospitals for filter
            var hospitals = _hospitalService.GetAllHospitals();
            ddlHospitalFilter.DataSource = hospitals;
            ddlHospitalFilter.DataTextField = "Name";
            ddlHospitalFilter.DataValueField = "Id";
            ddlHospitalFilter.DataBind();
            ddlHospitalFilter.Items.Insert(0, new ListItem("All Hospitals", ""));
        }

        private void LoadAppointments()
        {
            List<Appointment> appointments;

            if (IsAdmin)
            {
                appointments = _appointmentService.GetAllAppointments();
                appointments = ApplyFilters(appointments);
            }
            else
            {
                var patient = _patientService.GetPatientByUserId(CurrentUserId.Value);
                if (patient == null)
                {
                    lblMessage.Text = "Patient record not found.";
                    lblMessage.CssClass = "text-danger alert alert-danger";
                    return;
                }
                appointments = _appointmentService.GetAppointmentsByPatient(patient.Id);
            }

            var now = DateTime.Now;
            var allUpcoming = appointments.Where(a => a.AppointmentDate.Date > now.Date ||
                                                  (a.AppointmentDate.Date == now.Date && a.AppointmentTime > now.TimeOfDay))
                                          .OrderBy(a => a.AppointmentDate)
                                          .ThenBy(a => a.AppointmentTime)
                                          .ToList();

            var allPast = appointments.Where(a => a.AppointmentDate.Date < now.Date ||
                                              (a.AppointmentDate.Date == now.Date && a.AppointmentTime <= now.TimeOfDay))
                                      .OrderByDescending(a => a.AppointmentDate)
                                      .ThenByDescending(a => a.AppointmentTime)
                                      .ToList();

            // Apply pagination to upcoming appointments
            if (_showUpcoming)
            {
                int totalUpcoming = allUpcoming.Count;
                int totalPagesUpcoming = (int)Math.Ceiling((double)totalUpcoming / PageSizeUpcoming);

                // Ensure current page is valid
                if (CurrentPageUpcoming > totalPagesUpcoming && totalPagesUpcoming > 0)
                    CurrentPageUpcoming = totalPagesUpcoming;
                if (CurrentPageUpcoming < 1)
                    CurrentPageUpcoming = 1;

                var pagedUpcoming = allUpcoming
                    .Skip((CurrentPageUpcoming - 1) * PageSizeUpcoming)
                    .Take(PageSizeUpcoming)
                    .ToList();

                rptUpcomingAppointments.DataSource = pagedUpcoming;
                rptUpcomingAppointments.DataBind();
                lblNoUpcoming.Visible = !pagedUpcoming.Any();
                pnlUpcoming.Visible = true;
                pnlPast.Visible = false;

                // Update pagination controls for upcoming
                UpdateUpcomingPagination(totalUpcoming, totalPagesUpcoming);
            }
            else
            {
                // Apply pagination to past appointments
                int totalPast = allPast.Count;
                int totalPagesPast = (int)Math.Ceiling((double)totalPast / PageSizePast);

                // Ensure current page is valid
                if (CurrentPagePast > totalPagesPast && totalPagesPast > 0)
                    CurrentPagePast = totalPagesPast;
                if (CurrentPagePast < 1)
                    CurrentPagePast = 1;

                var pagedPast = allPast
                    .Skip((CurrentPagePast - 1) * PageSizePast)
                    .Take(PageSizePast)
                    .ToList();

                rptPastAppointments.DataSource = pagedPast;
                rptPastAppointments.DataBind();
                lblNoPast.Visible = !pagedPast.Any();
                pnlUpcoming.Visible = false;
                pnlPast.Visible = true;

                // Update pagination controls for past
                UpdatePastPagination(totalPast, totalPagesPast);
            }
        }

        private void UpdateUpcomingPagination(int totalCount, int totalPages)
        {
            lblCurrentPageUpcoming.Text = CurrentPageUpcoming.ToString();
            lblTotalPagesUpcoming.Text = totalPages.ToString();
            lblUpcomingCount.Text = $"Showing {((CurrentPageUpcoming - 1) * PageSizeUpcoming) + 1}-{Math.Min(CurrentPageUpcoming * PageSizeUpcoming, totalCount)} of {totalCount} appointments";

            // Update page size dropdown
            ddlPageSizeUpcoming.SelectedValue = PageSizeUpcoming.ToString();

            // Enable/disable navigation buttons
            lnkFirstUpcoming.Enabled = CurrentPageUpcoming > 1;
            lnkPrevUpcoming.Enabled = CurrentPageUpcoming > 1;
            lnkNextUpcoming.Enabled = CurrentPageUpcoming < totalPages;
            lnkLastUpcoming.Enabled = CurrentPageUpcoming < totalPages;

            // Show/hide pagination panel
            pnlUpcomingPagination.Visible = totalPages > 1;
        }

        private void UpdatePastPagination(int totalCount, int totalPages)
        {
            lblCurrentPagePast.Text = CurrentPagePast.ToString();
            lblTotalPagesPast.Text = totalPages.ToString();
            lblPastCount.Text = $"Showing {((CurrentPagePast - 1) * PageSizePast) + 1}-{Math.Min(CurrentPagePast * PageSizePast, totalCount)} of {totalCount} appointments";

            // Update page size dropdown
            ddlPageSizePast.SelectedValue = PageSizePast.ToString();

            // Enable/disable navigation buttons
            lnkFirstPast.Enabled = CurrentPagePast > 1;
            lnkPrevPast.Enabled = CurrentPagePast > 1;
            lnkNextPast.Enabled = CurrentPagePast < totalPages;
            lnkLastPast.Enabled = CurrentPagePast < totalPages;

            // Show/hide pagination panel
            pnlPastPagination.Visible = totalPages > 1;
        }

        // Add pagination event handlers
        protected void ddlPageSizeUpcoming_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageSizeUpcoming = int.Parse(ddlPageSizeUpcoming.SelectedValue);
            CurrentPageUpcoming = 1; // Reset to first page
            LoadAppointments();
        }

        protected void ddlPageSizePast_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageSizePast = int.Parse(ddlPageSizePast.SelectedValue);
            CurrentPagePast = 1; // Reset to first page
            LoadAppointments();
        }

        protected void lnkFirstUpcoming_Click(object sender, EventArgs e)
        {
            CurrentPageUpcoming = 1;
            LoadAppointments();
        }

        protected void lnkPrevUpcoming_Click(object sender, EventArgs e)
        {
            if (CurrentPageUpcoming > 1)
            {
                CurrentPageUpcoming--;
                LoadAppointments();
            }
        }

        protected void lnkNextUpcoming_Click(object sender, EventArgs e)
        {
            CurrentPageUpcoming++;
            LoadAppointments();
        }

        protected void lnkLastUpcoming_Click(object sender, EventArgs e)
        {
            // Calculate total pages
            List<Appointment> appointments = IsAdmin ?
                ApplyFilters(_appointmentService.GetAllAppointments()) :
                _appointmentService.GetAppointmentsByPatient(_patientService.GetPatientByUserId(CurrentUserId.Value).Id);

            var now = DateTime.Now;
            var allUpcoming = appointments.Where(a => a.AppointmentDate.Date > now.Date ||
                                                  (a.AppointmentDate.Date == now.Date && a.AppointmentTime > now.TimeOfDay))
                                          .ToList();

            int totalPages = (int)Math.Ceiling((double)allUpcoming.Count / PageSizeUpcoming);
            CurrentPageUpcoming = totalPages;
            LoadAppointments();
        }

        // Similar methods for Past appointments
        protected void lnkFirstPast_Click(object sender, EventArgs e)
        {
            CurrentPagePast = 1;
            LoadAppointments();
        }

        protected void lnkPrevPast_Click(object sender, EventArgs e)
        {
            if (CurrentPagePast > 1)
            {
                CurrentPagePast--;
                LoadAppointments();
            }
        }

        protected void lnkNextPast_Click(object sender, EventArgs e)
        {
            CurrentPagePast++;
            LoadAppointments();
        }

        protected void lnkLastPast_Click(object sender, EventArgs e)
        {
            // Calculate total pages
            List<Appointment> appointments = IsAdmin ?
                ApplyFilters(_appointmentService.GetAllAppointments()) :
                _appointmentService.GetAppointmentsByPatient(_patientService.GetPatientByUserId(CurrentUserId.Value).Id);

            var now = DateTime.Now;
            var allPast = appointments.Where(a => a.AppointmentDate.Date < now.Date ||
                                              (a.AppointmentDate.Date == now.Date && a.AppointmentTime <= now.TimeOfDay))
                                      .ToList();

            int totalPages = (int)Math.Ceiling((double)allPast.Count / PageSizePast);
            CurrentPagePast = totalPages;
            LoadAppointments();
        }

        private List<Appointment> ApplyFilters(List<Appointment> appointments)
        {
            // Filter by patient name
            if (!string.IsNullOrEmpty(txtPatientName.Text))
            {
                appointments = appointments.Where(a =>
                    a.Patient != null &&
                    a.Patient.Name.ToLower().Contains(txtPatientName.Text.ToLower())).ToList();
            }

            // Filter by doctor
            if (!string.IsNullOrEmpty(ddlDoctorFilter.SelectedValue))
            {
                int doctorId = int.Parse(ddlDoctorFilter.SelectedValue);
                appointments = appointments.Where(a => a.DoctorId == doctorId).ToList();
            }

            // Filter by hospital
            if (!string.IsNullOrEmpty(ddlHospitalFilter.SelectedValue))
            {
                int hospitalId = int.Parse(ddlHospitalFilter.SelectedValue);
                appointments = appointments.Where(a => a.HospitalId == hospitalId).ToList();
            }

            // Filter by status
            if (ddlStatusFilter.SelectedValue == "Upcoming")
            {
                var now = DateTime.Now;
                appointments = appointments.Where(a => a.AppointmentDate.Date > now.Date ||
                                                     (a.AppointmentDate.Date == now.Date && a.AppointmentTime > now.TimeOfDay)).ToList();
            }
            else if (ddlStatusFilter.SelectedValue == "Past")
            {
                var now = DateTime.Now;
                appointments = appointments.Where(a => a.AppointmentDate.Date < now.Date ||
                                                     (a.AppointmentDate.Date == now.Date && a.AppointmentTime <= now.TimeOfDay)).ToList();
            }

            // Filter by date range
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                DateTime fromDate = DateTime.Parse(txtFromDate.Text);
                appointments = appointments.Where(a => a.AppointmentDate.Date >= fromDate.Date).ToList();
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                DateTime toDate = DateTime.Parse(txtToDate.Text);
                appointments = appointments.Where(a => a.AppointmentDate.Date <= toDate.Date).ToList();
            }

            return appointments;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        protected void btnClearFilters_Click(object sender, EventArgs e)
        {
            txtPatientName.Text = "";
            ddlDoctorFilter.SelectedIndex = 0;
            ddlHospitalFilter.SelectedIndex = 0;
            ddlStatusFilter.SelectedIndex = 0;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            LoadAppointments();
        }

        protected void lnkUpcoming_Click(object sender, EventArgs e)
        {
            _showUpcoming = true;
            LoadAppointments();
        }

        protected void lnkPast_Click(object sender, EventArgs e)
        {
            _showUpcoming = false;
            LoadAppointments();
        }

        protected void rptAppointments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!IsAdmin)
            {
                lblMessage.Text = "You do not have permission to perform this action.";
                lblMessage.CssClass = "text-danger alert alert-danger";
                return;
            }

            int appointmentId = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Edit")
            {
                Response.Redirect($"~/Pages/EditAppointment.aspx?Id={appointmentId}");
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    _appointmentService.CancelAppointment(appointmentId);
                    lblMessage.Text = "Appointment cancelled successfully.";
                    lblMessage.CssClass = "text-success alert alert-success";
                    LoadAppointments();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error cancelling appointment: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
        }

        protected string FormatTime(TimeSpan time)
        {
            return DateTime.Today.Add(time).ToString("hh:mm tt");
        }

        protected bool GetIsAdmin()
        {
            return IsAdmin;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _appointmentService?.Dispose();
            _patientService?.Dispose();
            _doctorService?.Dispose();
            _hospitalService?.Dispose();
        }
    }
}