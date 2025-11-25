using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Durdans_WebForms_MVP.Models;
using Durdans_WebForms_MVP.Data;

namespace Durdans_WebForms_MVP.DAL
{
    public class AppointmentRepository : IDisposable
    {
        private ClinicDbContext _context;

        public AppointmentRepository()
        {
            _context = new ClinicDbContext();
        }

        public int InsertAppointment(Appointment appointment)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Check for conflicts (optional - can be done in BLL)
                    // Check for max bookings limit
                    var dayOfWeek = appointment.AppointmentDate.DayOfWeek.ToString();
                    var availability = _context.DoctorAvailabilities
                        .FirstOrDefault(da => da.DoctorId == appointment.DoctorId && 
                                            da.HospitalId == appointment.HospitalId && 
                                            da.DayOfWeek == dayOfWeek);

                    if (availability != null)
                    {
                        var existingBookingsCount = _context.Appointments.Count(a =>
                            a.DoctorId == appointment.DoctorId &&
                            a.HospitalId == appointment.HospitalId &&
                            DbFunctions.TruncateTime(a.AppointmentDate) == DbFunctions.TruncateTime(appointment.AppointmentDate) &&
                            a.AppointmentTime == appointment.AppointmentTime);

                        if (existingBookingsCount >= availability.MaxBookingsPerSlot)
                        {
                            throw new InvalidOperationException($"This time slot is fully booked (Max {availability.MaxBookingsPerSlot} patients).");
                        }
                    }

                    _context.Appointments.Add(appointment);
                    _context.SaveChanges();
                    transaction.Commit();
                    
                    return appointment.Id;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Hospital)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            return _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .Include(a => a.Hospital)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId, DateTime date)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId && 
                           DbFunctions.TruncateTime(a.AppointmentDate) == date.Date)
                .Include(a => a.Patient)
                .Include(a => a.Hospital)
                .OrderBy(a => a.AppointmentTime)
                .ToList();
        }

        public Appointment GetAppointmentById(int id)
        {
            return _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Hospital)
                .FirstOrDefault(a => a.Id == id);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            var existingAppointment = _context.Appointments.Find(appointment.Id);
            if (existingAppointment != null)
            {
                existingAppointment.DoctorId = appointment.DoctorId;
                existingAppointment.PatientId = appointment.PatientId;
                existingAppointment.HospitalId = appointment.HospitalId;
                existingAppointment.AppointmentDate = appointment.AppointmentDate;
                existingAppointment.AppointmentTime = appointment.AppointmentTime;
                existingAppointment.BookingType = appointment.BookingType;
                existingAppointment.BookedBy = appointment.BookedBy;
                
                _context.SaveChanges();
            }
        }

        public void DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
