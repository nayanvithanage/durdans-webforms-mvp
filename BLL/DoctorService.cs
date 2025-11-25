using System;
using System.Collections.Generic;
using System.Linq;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class DoctorService : IDisposable
    {
        private DoctorRepository _doctorRepo;
        private AvailabilityRepository _availabilityRepo;

        public DoctorService()
        {
            _doctorRepo = new DoctorRepository();
            _availabilityRepo = new AvailabilityRepository();
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepo.GetAllDoctors();
        }

        public List<Doctor> GetDoctorsBySpecialization(int specializationId)
        {
            if (specializationId <= 0)
            {
                throw new ArgumentException("Valid specialization ID is required.");
            }

            return _doctorRepo.GetDoctorsBySpecialization(specializationId);
        }
        public Doctor GetDoctorById(int id)
        {
            return _doctorRepo.GetDoctorById(id);
        }

        public int RegisterDoctor(Doctor doctor, List<int> hospitalIds = null)
        {
            // Business Logic: Validate doctor details
            if (string.IsNullOrWhiteSpace(doctor.Name))
            {
                throw new ArgumentException("Doctor name is required.");
            }

            if (doctor.SpecializationId <= 0)
            {
                throw new ArgumentException("Specialization is required.");
            }

            if (doctor.ConsultationFee <= 0)
            {
                throw new ArgumentException("Consultation fee must be greater than zero.");
            }

            return _doctorRepo.InsertDoctor(doctor, hospitalIds);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            if (doctor.Id <= 0)
            {
                throw new ArgumentException("Invalid doctor ID.");
            }

            _doctorRepo.UpdateDoctor(doctor);
        }

        public void DeleteDoctor(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid doctor ID.");
            }

            _doctorRepo.DeleteDoctor(id);
        }

        // Availability Management Methods
        public int AddDoctorAvailability(DoctorAvailability availability)
        {
            // Validate availability
            if (availability.DoctorId <= 0)
            {
                throw new ArgumentException("Invalid doctor ID.");
            }

            if (availability.HospitalId <= 0)
            {
                throw new ArgumentException("Invalid hospital ID.");
            }

            if (string.IsNullOrWhiteSpace(availability.DayOfWeek))
            {
                throw new ArgumentException("Day of week is required.");
            }

            if (availability.StartTime >= availability.EndTime)
            {
                throw new ArgumentException("Start time must be before end time.");
            }

            if (availability.MaxBookingsPerSlot <= 0)
            {
                throw new ArgumentException("Max bookings per slot must be greater than zero.");
            }

            return _availabilityRepo.InsertAvailability(availability);
        }

        public List<DoctorAvailability> GetDoctorAvailabilities(int doctorId)
        {
            return _availabilityRepo.GetAvailabilitiesByDoctor(doctorId);
        }

        public DoctorAvailability GetAvailabilityForDay(int doctorId, int hospitalId, string dayOfWeek)
        {
            return _availabilityRepo.GetAvailabilityForDoctorOnDay(doctorId, hospitalId, dayOfWeek);
        }

        public void UpdateAvailability(DoctorAvailability availability)
        {
            _availabilityRepo.UpdateAvailability(availability);
        }

        public void DeleteAvailability(int availabilityId)
        {
            _availabilityRepo.DeleteAvailability(availabilityId);
        }

        public void Dispose()
        {
            _doctorRepo?.Dispose();
            _availabilityRepo?.Dispose();
        }
    }
}
