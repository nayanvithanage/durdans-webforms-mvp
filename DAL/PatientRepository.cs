using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    public class PatientRepository
    {
        public int InsertPatient(Patient patient)
        {
            object result = SqlHelper.ExecuteScalar("sp_InsertPatient",
                new SqlParameter("@Name", patient.Name),
                new SqlParameter("@DateOfBirth", patient.DateOfBirth),
                new SqlParameter("@ContactNumber", patient.ContactNumber));
            return Convert.ToInt32(result);
        }

        public List<Patient> GetAllPatients()
        {
            DataTable dt = SqlHelper.ExecuteReader("sp_GetAllPatients");
            List<Patient> patients = new List<Patient>();
            foreach (DataRow row in dt.Rows)
            {
                patients.Add(new Patient
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    ContactNumber = row["ContactNumber"].ToString()
                });
            }
            return patients;
        }

        public Patient GetPatientById(int id)
        {
            DataTable dt = SqlHelper.ExecuteReader("sp_GetPatientById", new SqlParameter("@Id", id));
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new Patient
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    ContactNumber = row["ContactNumber"].ToString()
                };
            }
            return null;
        }
    }
}
