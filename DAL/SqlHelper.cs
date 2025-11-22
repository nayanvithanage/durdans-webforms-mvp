using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    // MOCK IMPLEMENTATION OF SQL HELPER
    // In a real app, this would use System.Data.SqlClient to execute Stored Procedures.
    // Here, we use static lists to simulate a database so the user can run it immediately.
    public static class SqlHelper
    {
        // Simulated Database Tables
        public static List<Doctor> Doctors = new List<Doctor>
        {
            new Doctor { Id = 1, Name = "Dr. Perera", Specialization = "Cardiology", ConsultationFee = 2500 },
            new Doctor { Id = 2, Name = "Dr. Silva", Specialization = "Pediatrics", ConsultationFee = 2000 },
            new Doctor { Id = 3, Name = "Dr. Fernando", Specialization = "Dermatology", ConsultationFee = 1800 }
        };

        public static List<Patient> Patients = new List<Patient>();
        public static List<Appointment> Appointments = new List<Appointment>();

        // Helper to simulate "Identity" column auto-increment
        public static int GetNextId<T>(List<T> list)
        {
            return list.Count + 1;
        }
    }
}
