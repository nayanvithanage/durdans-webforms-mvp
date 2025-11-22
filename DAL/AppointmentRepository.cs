using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    public class AppointmentRepository
    {
        public int InsertAppointment(Appointment appointment)
        {
            // In real app: return SqlHelper.ExecuteScalar("sp_InsertAppointment", params);
            appointment.Id = SqlHelper.GetNextId(SqlHelper.Appointments);
            SqlHelper.Appointments.Add(appointment);
            return appointment.Id;
        }

        public List<Appointment> GetAllAppointments()
        {
            // Manually populating navigation properties for display
            var appointments = SqlHelper.Appointments;
            foreach (var appt in appointments)
            {
                appt.Doctor = SqlHelper.Doctors.FirstOrDefault(d => d.Id == appt.DoctorId);
                appt.Patient = SqlHelper.Patients.FirstOrDefault(p => p.Id == appt.PatientId);
            }
            return appointments;
        }
    }
}
