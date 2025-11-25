using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Durdans_WebForms_MVP.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public int SpecializationId { get; set; }

        [Required]
        public decimal ConsultationFee { get; set; }

        // Navigation properties
        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Hospital> Hospitals { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<DoctorAvailability> Availabilities { get; set; }

        public Doctor()
        {
            Hospitals = new HashSet<Hospital>();
            Appointments = new HashSet<Appointment>();
            Availabilities = new HashSet<DoctorAvailability>();
        }
    }
}
