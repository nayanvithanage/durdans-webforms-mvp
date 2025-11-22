using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Durdans_WebForms_MVP.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        
        // Navigation properties (optional for ADO.NET but good for model)
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
