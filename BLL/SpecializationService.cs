using System;
using System.Collections.Generic;
using System.Linq;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class SpecializationService : IDisposable
    {
        private SpecializationRepository _specializationRepo;

        public SpecializationService()
        {
            _specializationRepo = new SpecializationRepository();
        }

        public List<Specialization> GetAllActiveSpecializations()
        {
            return _specializationRepo.GetAllSpecializations();
        }

        public List<Specialization> GetAllSpecializations()
        {
            return _specializationRepo.GetAllSpecializationsIncludingInactive();
        }

        public Specialization GetSpecializationById(int id)
        {
            return _specializationRepo.GetSpecializationById(id);
        }

        public int AddSpecialization(Specialization specialization)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(specialization.Name))
            {
                throw new ArgumentException("Specialization name is required.");
            }

            // Check for duplicates
            var existing = _specializationRepo.GetAllSpecializationsIncludingInactive();
            if (existing.Any(s => s.Name.Equals(specialization.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("A specialization with this name already exists.");
            }

            return _specializationRepo.InsertSpecialization(specialization);
        }

        public void UpdateSpecialization(Specialization specialization)
        {
            if (specialization.Id <= 0)
            {
                throw new ArgumentException("Invalid specialization ID.");
            }

            if (string.IsNullOrWhiteSpace(specialization.Name))
            {
                throw new ArgumentException("Specialization name is required.");
            }

            // Check for duplicates (excluding current record)
            var existing = _specializationRepo.GetAllSpecializationsIncludingInactive();
            if (existing.Any(s => s.Id != specialization.Id && 
                s.Name.Equals(specialization.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("A specialization with this name already exists.");
            }

            _specializationRepo.UpdateSpecialization(specialization);
        }

        public void DeleteSpecialization(int id)
        {
            _specializationRepo.DeleteSpecialization(id);
        }

        public void Dispose()
        {
            _specializationRepo?.Dispose();
        }
    }
}
