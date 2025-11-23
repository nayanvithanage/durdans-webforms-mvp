using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class PatientService
    {
        private PatientRepository _patientRepo = new PatientRepository();

        public int RegisterPatient(Patient patient)
        {
            // Business Logic: Check if patient already exists (omitted for MVP)
            return _patientRepo.InsertPatient(patient);
        }

        public List<Patient> GetAllPatients()
        {
            return _patientRepo.GetAllPatients();
        }
    }
}
