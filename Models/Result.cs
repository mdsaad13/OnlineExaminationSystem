using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models
{
    public class Result
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ExamID { get; set; }
        public DateTime AttendDate { get; set; }
        public ResultStatus Status { get; set; }

        public string Subject { get; set; }
        public string ExamTitle { get; set; }
    }

    public class ResultStatus
    {
        public int MarksScored { get; set; }

        /// <summary>
        /// 1-pass, 0-fail
        /// </summary>
        public int StatusInInt { get; set; }

        public string StatusInStr { get; set; }

        public double Percentage { get; set; }

        public int TotalMarks { get; set; }
    }

    public class ResultBundle
    {
        public Exam Exam { get; set; }

        public List<Question> AllQuestion { get; set; }

        public Result Result { get; set; }
    }
}