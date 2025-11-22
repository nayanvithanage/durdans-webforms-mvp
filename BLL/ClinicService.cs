using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class ClinicService
    {
        private DoctorRepository _doctorRepo = new DoctorRepository();
        private PatientRepository _patientRepo = new PatientRepository();
        private AppointmentRepository _appointmentRepo = new AppointmentRepository();

        // Doctor Methods
        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepo.GetAllDoctors();
        }

        public List<Doctor> GetDoctorsBySpecialization(string specialization)
        {
            return _doctorRepo.GetDoctorsBySpecialization(specialization);
        }

        // Patient Methods
        public int RegisterPatient(Patient patient)
        {
            // Business Logic: Check if patient already exists (omitted for MVP)
            return _patientRepo.InsertPatient(patient);
        }

        public List<Patient> GetAllPatients()
        {
            return _patientRepo.GetAllPatients();
        }

        // Appointment Methods
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
