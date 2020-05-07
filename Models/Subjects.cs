using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models
{
    public class Subjects
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Enter a valid name")]
        public string Name { get; set; }
    }
}