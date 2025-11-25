using System;
using System.ComponentModel.DataAnnotations;

namespace Durdans_WebForms_MVP.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } // "Admin", "Patient", etc.

        [StringLength(200)]
        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
