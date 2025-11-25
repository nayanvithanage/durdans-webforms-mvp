using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Durdans_WebForms_MVP.Models
{
    public class Hospital
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(20)]
        public string ContactNumber { get; set; }

        // Navigation properties
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<DoctorAvailability> DoctorAvailabilities { get; set; }

        public Hospital()
        {
            Doctors = new HashSet<Doctor>();
            Appointments = new HashSet<Appointment>();
            DoctorAvailabilities = new HashSet<DoctorAvailability>();
        }
    }
}
