using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    public class DoctorRepository
    {
        public List<Doctor> GetAllDoctors()
        {
            DataTable dt = SqlHelper.ExecuteReader("sp_GetAllDoctors");
            List<Doctor> doctors = new List<Doctor>();
            foreach (DataRow row in dt.Rows)
            {
                doctors.Add(MapToDoctor(row));
            }
            return doctors;
        }

        public List<Doctor> GetDoctorsBySpecialization(string specialization)
        {
            DataTable dt = SqlHelper.ExecuteReader("sp_GetDoctorsBySpecialization", new SqlParameter("@Specialization", specialization));
            List<Doctor> doctors = new List<Doctor>();
            foreach (DataRow row in dt.Rows)
            {
                doctors.Add(MapToDoctor(row));
            }
            return doctors;
        }

        public Doctor GetDoctorById(int id)
        {
            DataTable dt = SqlHelper.ExecuteReader("sp_GetDoctorById", new SqlParameter("@Id", id));
            if (dt.Rows.Count > 0)
            {
                return MapToDoctor(dt.Rows[0]);
            }
            return null;
        }

        public int InsertDoctor(Doctor doctor)
        {
            object result = SqlHelper.ExecuteScalar("sp_InsertDoctor",
                new SqlParameter("@Name", doctor.Name),
                new SqlParameter("@Specialization", doctor.Specialization),
                new SqlParameter("@ConsultationFee", doctor.ConsultationFee),
                new SqlParameter("@AvailableDays", (object)doctor.AvailableDays ?? DBNull.Value),
                new SqlParameter("@AvailableTime", (object)doctor.AvailableTime ?? DBNull.Value));
            
            return Convert.ToInt32(result);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            SqlHelper.ExecuteNonQuery("sp_UpdateDoctor",
                new SqlParameter("@Id", doctor.Id),
                new SqlParameter("@Name", doctor.Name),
                new SqlParameter("@Specialization", doctor.Specialization),
                new SqlParameter("@ConsultationFee", doctor.ConsultationFee),
                new SqlParameter("@AvailableDays", (object)doctor.AvailableDays ?? DBNull.Value),
                new SqlParameter("@AvailableTime", (object)doctor.AvailableTime ?? DBNull.Value));
        }

        private Doctor MapToDoctor(DataRow row)
        {
            return new Doctor
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Specialization = row["Specialization"].ToString(),
                ConsultationFee = Convert.ToDecimal(row["ConsultationFee"]),
                AvailableDays = row["AvailableDays"] != DBNull.Value ? row["AvailableDays"].ToString() : null,
                AvailableTime = row["AvailableTime"] != DBNull.Value ? row["AvailableTime"].ToString() : null
            };
        }
    }
}
