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
    }
}