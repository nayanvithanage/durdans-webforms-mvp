using System;
using System.Collections.Generic;
using System.Linq;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class AppointmentService : IDisposable
    {
        private AppointmentRepository _appointmentRepo;
        private AvailabilityRepository _availabilityRepo;

        public AppointmentService()
        {
            _appointmentRepo = new AppointmentRepository();
            _availabilityRepo = new AvailabilityRepository();
        }

        public int BookAppointment(Appointment appointment)
        {
            // Business Logic: Validate appointment date
            if (appointment.AppointmentDate.Date < DateTime.Now.Date)
            {
                throw new ArgumentException("Appointment date cannot be in the past.");
            }

            // Validate required fields
            if (appointment.DoctorId <= 0)
            {
                throw new ArgumentException("Doctor is required.");
            }

            if (appointment.PatientId <= 0)
            {
                throw new ArgumentException("Patient is required.");
            }

            if (appointment.HospitalId <= 0)
            {
                throw new ArgumentException("Hospital is required.");
            }

            // Business Logic: Check doctor availability for the selected day
            var dayOfWeek = appointment.AppointmentDate.DayOfWeek.ToString();
            var availability = _availabilityRepo.GetAvailabilityForDoctorOnDay(
                appointment.DoctorId, 
                appointment.HospitalId, 
                dayOfWeek);

            if (availability == null)
            {
                throw new InvalidOperationException("Doctor is not available on this day at this hospital.");
            }

            // Check if appointment time is within doctor's available hours
            if (appointment.AppointmentTime < availability.StartTime || 
                appointment.AppointmentTime >= availability.EndTime)
            {
                throw new InvalidOperationException($"Appointment time must be between {availability.StartTime} and {availability.EndTime}.");
            }

            // Check if slot is not fully booked
            var existingAppointments = _appointmentRepo.GetAppointmentsByDoctor(
                appointment.DoctorId, 
                appointment.AppointmentDate);

            var bookingsInSlot = existingAppointments.Count(a => a.AppointmentTime == appointment.AppointmentTime);

            if (bookingsInSlot >= availability.MaxBookingsPerSlot)
            {
                throw new InvalidOperationException("This time slot is fully booked.");
            }

            // Insert appointment (repository handles transaction and conflict check)
            return _appointmentRepo.InsertAppointment(appointment);
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointmentRepo.GetAllAppointments();
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            return _appointmentRepo.GetAppointmentsByPatient(patientId);
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId, DateTime date)
        {
            return _appointmentRepo.GetAppointmentsByDoctor(doctorId, date);
        }

        public Appointment GetAppointmentById(int id)
        {
            return _appointmentRepo.GetAppointmentById(id);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            if (appointment.Id <= 0)
            {
                throw new ArgumentException("Invalid appointment ID.");
            }

            _appointmentRepo.UpdateAppointment(appointment);
        }

        public void CancelAppointment(int appointmentId)
        {
            _appointmentRepo.DeleteAppointment(appointmentId);
        }

        public void Dispose()
        {
            _appointmentRepo?.Dispose();
            _availabilityRepo?.Dispose();
        }
    }
}
