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

        public void UpdatePatient(Patient patient)
        {
            if (patient.Id <= 0)
            {
                throw new ArgumentException("Invalid patient ID.");
            }

            _patientRepo.UpdatePatient(patient);
        }

        public void Dispose()
        {
            _patientRepo?.Dispose();
        }
    }
}
