using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    public class DoctorRepository
    {
        public List<Doctor> GetAllDoctors()
        {
            // In real app: return SqlHelper.ExecuteReader("sp_GetAllDoctors");
            return SqlHelper.Doctors;
        }

        public List<Doctor> GetDoctorsBySpecialization(string specialization)
        {
            return SqlHelper.Doctors.Where(d => d.Specialization == specialization).ToList();
        }

        public Doctor GetDoctorById(int id)
        {
            return SqlHelper.Doctors.FirstOrDefault(d => d.Id == id);
        }

        public int InsertDoctor(Doctor doctor)
        {
            doctor.Id = SqlHelper.GetNextId(SqlHelper.Doctors);
            SqlHelper.Doctors.Add(doctor);
            return doctor.Id;
        }

        public void UpdateDoctor(Doctor doctor)
        {
            var existing = GetDoctorById(doctor.Id);
            if (existing != null)
            {
                existing.Name = doctor.Name;
                existing.Specialization = doctor.Specialization;
                existing.ConsultationFee = doctor.ConsultationFee;
                existing.AvailableDays = doctor.AvailableDays;
                existing.AvailableTime = doctor.AvailableTime;
            }
        }
    }
}
