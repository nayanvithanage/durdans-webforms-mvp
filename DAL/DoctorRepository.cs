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
    }
}
