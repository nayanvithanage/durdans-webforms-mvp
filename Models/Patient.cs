using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Durdans_WebForms_MVP.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        public string ContactNumber { get; set; }

        // Link to User account (for patients who have user accounts)
        public int? UserId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }

        public Patient()
        {
            Appointments = new HashSet<Appointment>();
        }
    }
}
