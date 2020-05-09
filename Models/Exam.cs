using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Models
{
    public class Exam
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Enter a valid title")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        [AllowHtml]
        public string Description { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 50)]
        [AllowHtml]
        public string Rules { get; set; }

        [Required]
        public int SubID { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Enter a valid passing marks")]
        public int PassingMarks { get; set; }

        public DateTime DateTime { get; set; }

        public int Status { get; set; }

        public string SubjectName { get; set; }
    }

    public class Question
    {
        public int ID { get; set; }
        public int ExamID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Answer { get; set; }

        [Required]
        public string Option1 { get; set; }

        [Required]
        public string Option2 { get; set; }

        [Required]
        public string Option3 { get; set; }

        [Required]
        public string Option4 { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Enter a valid marks")]
        public int Marks { get; set; }

        public int UserAnswered { get; set; }
    }

    public class ExamBundle
    {
        public Exam Exam { get; set; }

        public List<Question> AllQuestion { get; set; }
    }
}