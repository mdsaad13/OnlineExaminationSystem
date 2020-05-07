using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models
{
    public class User
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

        [Required(ErrorMessage = "Kindly enter your password")]
        [Display(Name = "Password")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Enter a valid password")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!#$%^&_+-=]{6,16}$", ErrorMessage = "Enter a valid password")]
        public string Password { get; set; }

        [Display(Name = "Re-Enter Password")]
        [Compare("Password", ErrorMessage = "Entered Password did not match")]
        public string Re_Password { get; set; }

        public int Status { get; set; }

        public DateTime DateTime { get; set; }
    }

    public class Admin
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

        [Required(ErrorMessage = "Kindly enter your password")]
        [Display(Name = "Password")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Enter a valid password")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!#$%^&_+-=]{6,16}$", ErrorMessage = "Enter a valid password")]
        public string Password { get; set; }

        [Display(Name = "Re-Enter Password")]
        [Compare("Password", ErrorMessage = "Entered Password did not match")]
        public string Re_Password { get; set; }
    }
}