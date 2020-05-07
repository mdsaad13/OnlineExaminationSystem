using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models
{
    public class ContactUs
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Enter a valid name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression("^([6-9]{1})([0-9]{9})$", ErrorMessage = "Enter a valid mobile number")]
        public string Mobile { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a valid subject")]
        public string Sub { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Enter a valid message")]
        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public int Seen { get; set; }
    }
}