using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Durdans_WebForms_MVP.Models
{
    [Table("Specializations")]
    public class Specialization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
