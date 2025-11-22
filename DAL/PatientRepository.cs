using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    public class PatientRepository
    {
        public int InsertPatient(Patient patient)
        {
            // In real app: return SqlHelper.ExecuteScalar("sp_InsertPatient", params);
            patient.Id = SqlHelper.GetNextId(SqlHelper.Patients);
            SqlHelper.Patients.Add(patient);
            return patient.Id;
        }

        public List<Patient> GetAllPatients()
        {
            return SqlHelper.Patients;
        }

        public Patient GetPatientById(int id)
        {
            return SqlHelper.Patients.FirstOrDefault(p => p.Id == id);
        }
    }
}
