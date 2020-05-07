using OnlineExaminationSystem.DAL;
using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    [StudentAuthorize]
    public class StudentController : Controller
    {
        public StudentController()
        {
            ViewBag.DashBoardType = "Student";
        }

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
                
        [HttpGet]
        public ActionResult Exams(int Subject = 0)
        {
            AdminUtil adminUtil = new AdminUtil();
            ViewBag.AllSubjects = adminUtil.AllSubjects();
            
            if (Subject == 0)
            {
                ViewBag.SelectedSubject = "All Subjects";
                ViewBag.SelectedSubjectActiveID = 0;
                return View(adminUtil.AllExams(true));
            }
            else
            {
                ViewBag.SelectedSubjectActiveID = Subject;
                ViewBag.SelectedSubject = adminUtil.GetSubjectByID(Subject).Name;
                return View("Exams", adminUtil.AllExams(true, Subject));
            }            
        }
    }
}