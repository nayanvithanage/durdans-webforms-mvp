using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Durdans_WebForms_MVP.Data;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    public class SpecializationRepository : IDisposable
    {
        private ClinicDbContext _context;

        public SpecializationRepository()
        {
            _context = new ClinicDbContext();
        }

        public List<Specialization> GetAllSpecializations()
        {
            return _context.Specializations
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToList();
        }

        public List<Specialization> GetAllSpecializationsIncludingInactive()
        {
            return _context.Specializations
                .OrderBy(s => s.Name)
                .ToList();
        }

        public Specialization GetSpecializationById(int id)
        {
            return _context.Specializations.Find(id);
        }

        public int InsertSpecialization(Specialization specialization)
        {
            _context.Specializations.Add(specialization);
            _context.SaveChanges();
            return specialization.Id;
        }

        public void UpdateSpecialization(Specialization specialization)
        {
            var existing = _context.Specializations.Find(specialization.Id);
            if (existing != null)
            {
                existing.Name = specialization.Name;
                existing.Description = specialization.Description;
                existing.IsActive = specialization.IsActive;
                _context.SaveChanges();
            }
        }

        public void DeleteSpecialization(int id)
        {
            var specialization = _context.Specializations.Find(id);
            if (specialization != null)
            {
                // Soft delete - just mark as inactive
                specialization.IsActive = false;
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
