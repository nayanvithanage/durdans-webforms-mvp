using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Durdans_WebForms_MVP.Models;
using Durdans_WebForms_MVP.Data;

namespace Durdans_WebForms_MVP.DAL
{
    public class PatientRepository : IDisposable
    {
        private ClinicDbContext _context;

        public PatientRepository()
        {
            _context = new ClinicDbContext();
        }

        public int InsertPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
            return patient.Id;
        }

        public List<Patient> GetAllPatients()
        {
            return _context.Patients
                .Include(p => p.Appointments)
                .Include(p => p.User)
                .ToList();
        }

        public Patient GetPatientById(int id)
        {
            return _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefault(p => p.Id == id);
        }

        public Patient GetPatientByUserId(int userId)
        {
            return _context.Patients
                .Include(p => p.Appointments)
                .FirstOrDefault(p => p.UserId == userId);
        }

        public void UpdatePatient(Patient patient)
        {
            var existingPatient = _context.Patients.Find(patient.Id);
            if (existingPatient != null)
            {
                existingPatient.Name = patient.Name;
                existingPatient.DateOfBirth = patient.DateOfBirth;
                existingPatient.ContactNumber = patient.ContactNumber;
                
                _context.SaveChanges();
            }
        }

        public void DeletePatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
