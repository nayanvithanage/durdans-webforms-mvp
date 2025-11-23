using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class AppointmentService
    {
        private AppointmentRepository _appointmentRepo = new AppointmentRepository();

        public void BookAppointment(Appointment appointment)
        {
            // Business Logic: Validate Date
            if (appointment.AppointmentDate <= DateTime.Now)
            {
                throw new Exception("Appointment date must be in the future.");
            }

            // Business Logic: Check Doctor Availability (omitted for MVP)

            _appointmentRepo.InsertAppointment(appointment);
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointmentRepo.GetAllAppointments();
        }
    }
}
