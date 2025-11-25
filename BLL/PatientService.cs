using System;
using System.Collections.Generic;
using System.Linq;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class PatientService : IDisposable
    {
        private PatientRepository _patientRepo;

        public PatientService()
        {
            _patientRepo = new PatientRepository();
        }

        public int RegisterPatient(Patient patient)
        {
            // Business Logic: Validate patient data
            if (string.IsNullOrWhiteSpace(patient.Name))
            {
                throw new ArgumentException("Patient name is required.");
            }

            if (string.IsNullOrWhiteSpace(patient.ContactNumber))
            {
                throw new ArgumentException("Contact number is required.");
            }

            if (patient.DateOfBirth >= DateTime.Now)
            {
                throw new ArgumentException("Date of birth must be in the past.");
            }

            // Check for duplicate contact number (optional validation)
            var existingPatients = _patientRepo.GetAllPatients();
            if (existingPatients.Any(p => p.ContactNumber == patient.ContactNumber))
            {
                throw new InvalidOperationException("A patient with this contact number already exists.");
            }

            return _patientRepo.InsertPatient(patient);
        }

        public List<Patient> GetAllPatients()
        {
            return _patientRepo.GetAllPatients();
        }

        public Patient GetPatientById(int id)
        {
            return _patientRepo.GetPatientById(id);
        }

        /// <summary>
        /// Gets a patient by name (used for matching logged-in user to patient record)
        /// Note: This is a temporary solution. Ideally, User and Patient should be linked via UserId
        /// </summary>
        public Patient GetPatientByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var patients = _patientRepo.GetAllPatients();
            return patients.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets a patient by user ID (preferred method for user-patient linking)
        /// </summary>
        public Patient GetPatientByUserId(int userId)
        {
            return _patientRepo.GetPatientByUserId(userId);
        }

        public void UpdatePatient(Patient patient)
        {
            if (patient.Id <= 0)
            {
                throw new ArgumentException("Invalid patient ID.");
            }

            // Validate updated data
            if (string.IsNullOrWhiteSpace(patient.Name))
            {
                throw new ArgumentException("Patient name is required.");
            }

            if (string.IsNullOrWhiteSpace(patient.ContactNumber))
            {
                throw new ArgumentException("Contact number is required.");
            }

            if (patient.DateOfBirth >= DateTime.Now)
            {
                throw new ArgumentException("Date of birth must be in the past.");
            }

            _patientRepo.UpdatePatient(patient);
        }

        public void DeletePatient(int patientId, AppointmentService appointmentService)
        {
            if (patientId <= 0)
            {
                throw new ArgumentException("Invalid patient ID.");
            }

            // Check if patient has appointments
            var appointments = appointmentService.GetAppointmentsByPatient(patientId);
            if (appointments != null && appointments.Any())
            {
                throw new InvalidOperationException($"Cannot delete patient. Patient has {appointments.Count} appointment(s). Please delete or cancel all appointments first.");
            }

            _patientRepo.DeletePatient(patientId);
        }

        public void Dispose()
        {
            _patientRepo?.Dispose();
        }
    }
}
