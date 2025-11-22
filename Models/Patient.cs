using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Durdans_WebForms_MVP.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactNumber { get; set; }
    }
}
