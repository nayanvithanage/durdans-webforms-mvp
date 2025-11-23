using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Durdans_WebForms_MVP.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public decimal ConsultationFee { get; set; }
        public string AvailableDays { get; set; } // e.g., "Monday, Wednesday"
        public string AvailableTime { get; set; } // e.g., "09:00 AM - 12:00 PM"
    }
}
