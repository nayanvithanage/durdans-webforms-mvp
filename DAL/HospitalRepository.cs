using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Durdans_WebForms_MVP.Models;
using Durdans_WebForms_MVP.Data;

namespace Durdans_WebForms_MVP.DAL
{
    public class HospitalRepository : IDisposable
    {
        private ClinicDbContext _context;

        public HospitalRepository()
        {
            _context = new ClinicDbContext();
        }

        public List<Hospital> GetAllHospitals()
        {
            return _context.Hospitals
                .Include(h => h.Doctors)
                .ToList();
        }

        public Hospital GetHospitalById(int id)
        {
            return _context.Hospitals
                .Include(h => h.Doctors)
                .Include(h => h.DoctorAvailabilities)
                .FirstOrDefault(h => h.Id == id);
        }

        public int InsertHospital(Hospital hospital)
        {
            _context.Hospitals.Add(hospital);
            _context.SaveChanges();
            return hospital.Id;
        }

        public void UpdateHospital(Hospital hospital)
        {
            var existingHospital = _context.Hospitals.Find(hospital.Id);
            if (existingHospital != null)
            {
                existingHospital.Name = hospital.Name;
                existingHospital.Address = hospital.Address;
                existingHospital.ContactNumber = hospital.ContactNumber;
                
                _context.SaveChanges();
            }
        }

        public void DeleteHospital(int id)
        {
            var hospital = _context.Hospitals.Find(id);
            if (hospital != null)
            {
                _context.Hospitals.Remove(hospital);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
