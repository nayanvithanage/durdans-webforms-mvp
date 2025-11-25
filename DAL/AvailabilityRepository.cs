using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Durdans_WebForms_MVP.Models;
using Durdans_WebForms_MVP.Data;

namespace Durdans_WebForms_MVP.DAL
{
    public class AvailabilityRepository : IDisposable
    {
        private ClinicDbContext _context;

        public AvailabilityRepository()
        {
            _context = new ClinicDbContext();
        }

        public List<DoctorAvailability> GetAvailabilitiesByDoctor(int doctorId)
        {
            return _context.DoctorAvailabilities
                .Where(da => da.DoctorId == doctorId)
                .Include(da => da.Hospital)
                .Include(da => da.Doctor)
                .ToList();
        }

        public DoctorAvailability GetAvailabilityForDoctorOnDay(int doctorId, int hospitalId, string dayOfWeek)
        {
            return _context.DoctorAvailabilities
                .FirstOrDefault(da => da.DoctorId == doctorId && 
                                     da.HospitalId == hospitalId && 
                                     da.DayOfWeek == dayOfWeek);
        }

        public int InsertAvailability(DoctorAvailability availability)
        {
            _context.DoctorAvailabilities.Add(availability);
            _context.SaveChanges();
            return availability.Id;
        }

        public void UpdateAvailability(DoctorAvailability availability)
        {
            var existing = _context.DoctorAvailabilities.Find(availability.Id);
            if (existing != null)
            {
                existing.DayOfWeek = availability.DayOfWeek;
                existing.StartTime = availability.StartTime;
                existing.EndTime = availability.EndTime;
                existing.MaxBookingsPerSlot = availability.MaxBookingsPerSlot;
                
                _context.SaveChanges();
            }
        }

        public void DeleteAvailability(int id)
        {
            var availability = _context.DoctorAvailabilities.Find(id);
            if (availability != null)
            {
                _context.DoctorAvailabilities.Remove(availability);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
