using System;
using System.Collections.Generic;
using System.Linq;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class HospitalService : IDisposable
    {
        private HospitalRepository _hospitalRepo;

        public HospitalService()
        {
            _hospitalRepo = new HospitalRepository();
        }

        public List<Hospital> GetAllHospitals()
        {
            return _hospitalRepo.GetAllHospitals();
        }

        public Hospital GetHospitalById(int id)
        {
            return _hospitalRepo.GetHospitalById(id);
        }

        public int RegisterHospital(Hospital hospital)
        {
            // Business Logic: Validate hospital details
            if (string.IsNullOrWhiteSpace(hospital.Name))
            {
                throw new ArgumentException("Hospital name is required.");
            }

            if (string.IsNullOrWhiteSpace(hospital.Address))
            {
                throw new ArgumentException("Hospital address is required.");
            }

            return _hospitalRepo.InsertHospital(hospital);
        }

        // Alias method for consistency with page naming
        public int AddHospital(Hospital hospital)
        {
            return RegisterHospital(hospital);
        }

        public void UpdateHospital(Hospital hospital)
        {
            if (hospital.Id <= 0)
            {
                throw new ArgumentException("Invalid hospital ID.");
            }

            _hospitalRepo.UpdateHospital(hospital);
        }

        public void DeleteHospital(int id)
        {
            _hospitalRepo.DeleteHospital(id);
        }

        public void Dispose()
        {
            _hospitalRepo?.Dispose();
        }
    }
}
