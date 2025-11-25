using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Durdans_WebForms_MVP.Models
{
    public class DoctorAvailability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        [StringLength(20)]
        public string DayOfWeek { get; set; } // "Monday", "Tuesday", etc.

        [Required]
        public TimeSpan StartTime { get; set; } // e.g., 09:00

        [Required]
        public TimeSpan EndTime { get; set; } // e.g., 17:00

        [Required]
        public int MaxBookingsPerSlot { get; set; } // e.g., 3

        // Navigation properties
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
