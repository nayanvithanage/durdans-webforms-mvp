using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Durdans_WebForms_MVP.Models;
using Durdans_WebForms_MVP.Data;

namespace Durdans_WebForms_MVP.DAL
{
    public class DoctorRepository : IDisposable
    {
        private ClinicDbContext _context;

        public DoctorRepository()
        {
            _context = new ClinicDbContext();
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors
                .Include(d => d.Specialization)
                .Include(d => d.Hospitals)
                .Include(d => d.Availabilities)
                .ToList();
        }

        public List<Doctor> GetDoctorsBySpecialization(int specializationId)
        {
            return _context.Doctors
                .Where(d => d.SpecializationId == specializationId)
                .Include(d => d.Specialization)
                .Include(d => d.Hospitals)
                .Include(d => d.Availabilities)
                .ToList();
        }

        public Doctor GetDoctorById(int id)
        {
            return _context.Doctors
                .Include(d => d.Specialization)
                .Include(d => d.Hospitals)
                .Include(d => d.Availabilities)
                .Include(d => d.Appointments)
                .FirstOrDefault(d => d.Id == id);
        }

        public int InsertDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
            return doctor.Id;
        }

        public int InsertDoctor(Doctor doctor, List<int> hospitalIds)
        {
            if (hospitalIds != null && hospitalIds.Any())
            {
                doctor.Hospitals = new List<Hospital>();
                foreach (var id in hospitalIds)
                {
                    var hospital = _context.Hospitals.Find(id);
                    if (hospital != null)
                    {
                        doctor.Hospitals.Add(hospital);
                    }
                }
            }

            _context.Doctors.Add(doctor);
            _context.SaveChanges();
            return doctor.Id;
        }

        public void UpdateDoctor(Doctor doctor)
        {
            var existingDoctor = _context.Doctors.Find(doctor.Id);
            if (existingDoctor != null)
            {
                existingDoctor.Name = doctor.Name;
                existingDoctor.SpecializationId = doctor.SpecializationId;
                existingDoctor.ConsultationFee = doctor.ConsultationFee;
                
                _context.SaveChanges();
            }
        }

        public void DeleteDoctor(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
