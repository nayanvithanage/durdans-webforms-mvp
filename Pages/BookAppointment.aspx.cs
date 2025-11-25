using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Durdans_WebForms_MVP.BLL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.Pages
{
    public partial class BookAppointment : BasePage
    {
        private AppointmentService _appointmentService;
        private DoctorService _doctorService;
        private PatientService _patientService;
        private HospitalService _hospitalService;
        private SpecializationService _specializationService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _patientService = new PatientService();
            _doctorService = new DoctorService();
            _appointmentService = new AppointmentService();
            _hospitalService = new HospitalService();
            _specializationService = new SpecializationService();

            // Configure patient validator based on user role (for both initial load and postback)
            if (IsAdmin)
            {
                rfvPatient.Enabled = true;
            }
            else
            {
                rfvPatient.Enabled = false;
            }

            if (!IsPostBack)
            {
                // Check user role and configure patient selection accordingly
                if (IsAdmin)
                {
                    // Admin can select any patient
                    ddlPatient.Visible = true;
                    lblPatient.Visible = true;
                    lblPatient.Text = "Select Patient";
                    pnlPatientInfo.Visible = false;
                    rfvPatient.Enabled = true;  // Enable validator for admin
                    LoadPatients();
                }
                else
                {
                    // Regular user (Patient) - show their patient information
                    ddlPatient.Visible = false;
                    lblPatient.Visible = true;
                    lblPatient.Text = "Patient Information";
                    pnlPatientInfo.Visible = true;
                    rfvPatient.Enabled = false;  // Disable validator for normal users

                    // Load and display logged-in user's patient information
                    LoadCurrentUserPatient();
                }

                LoadSpecializations();
            }
        }

        private void LoadPatients()
        {
            try
            {
                ddlPatient.DataSource = _patientService.GetAllPatients();
                ddlPatient.DataBind();
                ddlPatient.Items.Insert(0, new ListItem("-- Select Patient --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading patients: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        private void LoadCurrentUserPatient()
        {
            try
            {
                if (CurrentUserId.HasValue)
                {
                    var patient = _patientService.GetPatientByUserId(CurrentUserId.Value);
                    
                    if (patient != null)
                    {
                        lblPatientName.Text = patient.Name;
                        lblPatientContact.Text = patient.ContactNumber;
                        lblPatientDOB.Text = patient.DateOfBirth.ToString("dd MMM yyyy");
                    }
                    else
                    {
                        // Patient record not found
                        pnlPatientInfo.Visible = false;
                        lblMessage.Text = "Patient record not found. Please contact administrator to create your patient profile.";
                        lblMessage.CssClass = "text-danger alert alert-danger";
                    }
                }
                else
                {
                    pnlPatientInfo.Visible = false;
                    lblMessage.Text = "User information not available. Please log in again.";
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                pnlPatientInfo.Visible = false;
                lblMessage.Text = "Error loading patient information: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        private void LoadSpecializations()
        {
            try
            {
                var specializations = _specializationService.GetAllActiveSpecializations();
                ddlSpecialization.DataSource = specializations;
                ddlSpecialization.DataBind();
                ddlSpecialization.Items.Insert(0, new ListItem("-- Select Specialization --", ""));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading specializations: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        protected void ddlSpecialization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSpecialization.SelectedValue))
            {
                try
                {
                    int specializationId = int.Parse(ddlSpecialization.SelectedValue);
                    var doctors = _doctorService.GetDoctorsBySpecialization(specializationId);
                    ddlDoctor.DataSource = doctors;
                    ddlDoctor.DataBind();
                    ddlDoctor.Items.Insert(0, new ListItem("-- Select Doctor --", ""));
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error loading doctors: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
            else
            {
                ddlDoctor.Items.Clear();
                ddlDoctor.Items.Insert(0, new ListItem("-- Select Doctor --", ""));
            }

            // Clear hospital and time slots
            ddlHospital.Items.Clear();
            ddlHospital.Items.Insert(0, new ListItem("-- Select Hospital --", ""));
            pnlTimeSlots.Visible = false;
            pnlNoSlots.Visible = false;
        }

        protected void ddlDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlDoctor.SelectedValue))
            {
                try
                {
                    int doctorId = int.Parse(ddlDoctor.SelectedValue);
                    var doctor = _doctorService.GetDoctorById(doctorId);

                    if (doctor != null && doctor.Hospitals != null && doctor.Hospitals.Any())
                    {
                        ddlHospital.DataSource = doctor.Hospitals;
                        ddlHospital.DataBind();
                        ddlHospital.Items.Insert(0, new ListItem("-- Select Hospital --", ""));
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error loading hospitals: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
        }

        protected void calAppointmentDate_DayRender(object sender, DayRenderEventArgs e)
        {
            // Disable past dates
            if (e.Day.Date < DateTime.Today)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
                e.Cell.Font.Strikeout = true;
            }
        }

        protected void calAppointmentDate_SelectionChanged(object sender, EventArgs e)
        {
            // Clear time slots when date changes
            pnlTimeSlots.Visible = false;
            pnlNoSlots.Visible = false;
            hfSelectedTimeSlot.Value = "";
            lblSelectedSlot.Visible = false;
        }

        protected void LoadTimeSlots_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlDoctor.SelectedValue) ||
                string.IsNullOrEmpty(ddlHospital.SelectedValue) ||
                calAppointmentDate.SelectedDate == DateTime.MinValue)
            {
                lblMessage.Text = "Please select Doctor, Hospital, and Date first.";
                lblMessage.CssClass = "text-warning alert alert-warning";
                return;
            }

            try
            {
                int doctorId = int.Parse(ddlDoctor.SelectedValue);
                int hospitalId = int.Parse(ddlHospital.SelectedValue);
                DateTime selectedDate = calAppointmentDate.SelectedDate;

                // Get doctor's availability for the selected day
                var dayOfWeek = selectedDate.DayOfWeek.ToString();
                var availability = _doctorService.GetAvailabilityForDay(doctorId, hospitalId, dayOfWeek);

                if (availability == null)
                {
                    pnlTimeSlots.Visible = false;
                    pnlNoSlots.Visible = true;
                    lblMessage.Text = "Doctor is not available on this day at this hospital.";
                    lblMessage.CssClass = "text-warning alert alert-warning";
                    return;
                }

                // Get existing appointments
                var existingAppointments = _appointmentService.GetAppointmentsByDoctor(doctorId, selectedDate);

                // Generate time slots
                var timeSlots = GenerateTimeSlots(availability.StartTime, availability.EndTime, 
                    existingAppointments, availability.MaxBookingsPerSlot);

                if (timeSlots.Any())
                {
                    rptTimeSlots.DataSource = timeSlots;
                    rptTimeSlots.DataBind();
                    pnlTimeSlots.Visible = true;
                    pnlNoSlots.Visible = false;
                }
                else
                {
                    pnlTimeSlots.Visible = false;
                    pnlNoSlots.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading time slots: " + ex.Message;
                lblMessage.CssClass = "text-danger alert alert-danger";
            }
        }

        private List<TimeSlotInfo> GenerateTimeSlots(TimeSpan startTime, TimeSpan endTime, 
            List<Appointment> existingAppointments, int maxBookings)
        {
            var slots = new List<TimeSlotInfo>();
            var current = startTime;

            // Generate slots in 1-hour intervals (as per technical spec: 6:15PM-7:15PM format)
            while (current.Add(TimeSpan.FromHours(1)) <= endTime)
            {
                var bookingsInSlot = existingAppointments.Count(a => a.AppointmentTime == current);

                string status;
                string cssClass;

                if (bookingsInSlot == 0)
                {
                    status = "Available";
                    cssClass = "time-slot-available";
                }
                else if (bookingsInSlot < maxBookings)
                {
                    status = $"Almost Full ({bookingsInSlot}/{maxBookings})";
                    cssClass = "time-slot-almost-full";
                }
                else
                {
                    status = "Unavailable";
                    cssClass = "time-slot-unavailable";
                }

                var endSlotTime = current.Add(TimeSpan.FromHours(1));
                slots.Add(new TimeSlotInfo
                {
                    TimeValue = current.ToString(@"hh\:mm"),
                    DisplayText = $"{DateTime.Today.Add(current):hh:mm tt} - {DateTime.Today.Add(endSlotTime):hh:mm tt}",
                    Status = status,
                    CssClass = cssClass,
                    IsAvailable = bookingsInSlot < maxBookings
                });

                current = current.Add(TimeSpan.FromMinutes(15)); // Move by 15 minutes for next slot
            }

            return slots;
        }

        protected void rptTimeSlots_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectTimeSlot")
            {
                hfSelectedTimeSlot.Value = e.CommandArgument.ToString();
                lblSelectedSlot.Text = $"Selected Time Slot: {e.CommandArgument}";
                lblSelectedSlot.Visible = true;
                lblSelectedSlot.CssClass = "alert alert-success";
            }
        }

        protected void Book_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(hfSelectedTimeSlot.Value))
                    {
                        lblMessage.Text = "Please select a time slot.";
                        lblMessage.CssClass = "text-warning alert alert-warning";
                        return;
                    }

                    // Determine patient ID based on user role
                    int patientId;
                    string bookingType;
                    string bookedBy = CurrentUsername; // From BasePage

                    if (IsAdmin)
                    {
                        // Admin selects patient from dropdown
                        if (string.IsNullOrEmpty(ddlPatient.SelectedValue))
                        {
                            lblMessage.Text = "Please select a patient.";
                            lblMessage.CssClass = "text-warning alert alert-warning";
                            return;
                        }
                        patientId = int.Parse(ddlPatient.SelectedValue);
                        bookingType = "Admin";
                    }
                    else
                    {
                        // Regular user books for themselves
                        // Get patient by UserId (proper foreign key relationship)
                        var patient = _patientService.GetPatientByUserId(CurrentUserId.Value);
                        
                        if (patient == null)
                        {
                            lblMessage.Text = "Patient record not found. Please contact administrator to create your patient profile.";
                            lblMessage.CssClass = "text-danger alert alert-danger";
                            return;
                        }
                        
                        patientId = patient.Id;
                        bookingType = "Patient";
                    }

                    var appointment = new Appointment
                    {
                        PatientId = patientId,
                        DoctorId = int.Parse(ddlDoctor.SelectedValue),
                        HospitalId = int.Parse(ddlHospital.SelectedValue),
                        AppointmentDate = calAppointmentDate.SelectedDate,
                        AppointmentTime = TimeSpan.Parse(hfSelectedTimeSlot.Value),
                        BookingType = bookingType,
                        BookedBy = bookedBy
                    };

                    _appointmentService.BookAppointment(appointment);
                    lblMessage.Text = "Appointment Booked Successfully!";
                    lblMessage.CssClass = "text-success alert alert-success";

                    // Clear form
                    ClearForm();
                }
                catch (ArgumentException ex)
                {
                    lblMessage.Text = "Validation Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
                catch (InvalidOperationException ex)
                {
                    lblMessage.Text = "Booking Error: " + ex.Message;
                    lblMessage.CssClass = "text-warning alert alert-warning";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger alert alert-danger";
                }
            }
        }

        private void ClearForm()
        {
            // Only clear patient dropdown for admin users
            if (IsAdmin && ddlPatient.Visible)
            {
                ddlPatient.SelectedIndex = 0;
            }
            
            ddlSpecialization.SelectedIndex = 0;
            ddlDoctor.Items.Clear();
            ddlDoctor.Items.Insert(0, new ListItem("-- Select Doctor --", ""));
            ddlHospital.Items.Clear();
            ddlHospital.Items.Insert(0, new ListItem("-- Select Hospital --", ""));
            calAppointmentDate.SelectedDate = DateTime.MinValue;
            hfSelectedTimeSlot.Value = "";
            pnlTimeSlots.Visible = false;
            pnlNoSlots.Visible = false;
            lblSelectedSlot.Visible = false;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            _patientService?.Dispose();
            _doctorService?.Dispose();
            _appointmentService?.Dispose();
            _hospitalService?.Dispose();
            _specializationService?.Dispose();
        }

        // Helper class for time slot display
        public class TimeSlotInfo
        {
            public string TimeValue { get; set; }
            public string DisplayText { get; set; }
            public string Status { get; set; }
            public string CssClass { get; set; }
            public bool IsAvailable { get; set; }
        }
    }
}
