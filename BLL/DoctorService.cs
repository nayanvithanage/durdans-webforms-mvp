using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class DoctorService
    {
        private DoctorRepository _doctorRepo = new DoctorRepository();

        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepo.GetAllDoctors();
        }

        public List<Doctor> GetDoctorsBySpecialization(string specialization)
        {
            return _doctorRepo.GetDoctorsBySpecialization(specialization);
        }

        public int RegisterDoctor(Doctor doctor)
        {
            // Business Logic: Validate doctor details (omitted for MVP)
            return _doctorRepo.InsertDoctor(doctor);
        }

        public void UpdateDoctorAvailability(int doctorId, string days, string time)
        {
            var doctor = _doctorRepo.GetDoctorById(doctorId);
            if (doctor != null)
            {
                doctor.AvailableDays = days;
                doctor.AvailableTime = time;
                _doctorRepo.UpdateDoctor(doctor);
            }
            else
            {
                throw new Exception("Doctor not found.");
            }
        }
    }
}
