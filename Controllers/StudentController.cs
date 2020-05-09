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

        private void ClearOldExamCache()
        {
            Session["ExamID"] = null;
            Session["ResultID"] = null;
        }

        public ActionResult Index()
        {
            ClearOldExamCache();
            General general = new General();
            ViewBag.ExamsCount = general.CountByArgs("exams", "status = 1");
            ViewBag.UpcommingExams = general.CountByArgs("exams", "status = 0");
            ViewBag.ExamsAttended = general.CountByArgs("results", "user_id = "+ Convert.ToInt32(Session["StudentID"]));
            return View();
        }
                
        [HttpGet]
        public ActionResult Exams(int Subject = 0)
        {
            ClearOldExamCache();
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

        public ActionResult ViewExam(int ID)
        {
            ClearOldExamCache();
            AdminUtil adminUtil = new AdminUtil();
            return View(adminUtil.GetExamByID(ID));
        }

        [HttpPost]
        public JsonResult RegisterExam(Result result)
        {
            ClearOldExamCache();
            bool Status = false;
            StudentUtil studentUtil = new StudentUtil();

            result.ID = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            result.UserID = Convert.ToInt32(Session["StudentID"]);
            result.AttendDate = DateTime.Now;
            if (studentUtil.NewExam(result))
            {
                Session["ExamID"] = result.ExamID;
                Session["ResultID"] = result.ID;
                Status = true;
            }
            return Json(new { Status });
        }

        public ActionResult AttendExam()
        {
            if (Session["ExamID"] != null && Session["ResultID"] != null)
            {
                int ExamID = Convert.ToInt32(Session["ExamID"]);
                int ResultID = Convert.ToInt32(Session["ResultID"]);
                int StudentID = Convert.ToInt32(Session["StudentID"]);
                AdminUtil adminUtil = new AdminUtil();

                ExamBundle examBundle = new ExamBundle
                {
                    Exam = adminUtil.GetExamByID(ExamID),
                    AllQuestion = adminUtil.AllExamQuestions(ExamID, true)
                };

                return View(examBundle);
            }
            else
            {
                ViewBag.ErrorMessage = "An error occured while loading exam!<br>Kindly try again later";
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AttendExam(FormCollection formCollection)
        {
            if (Session["ExamID"] != null && Session["ResultID"] != null)
            {
                StudentUtil studentUtil = new StudentUtil();
                int ResultID = Convert.ToInt32(Session["ResultID"]);
                int TypecastedQuesID;
                int TypecastedAnsID;
                foreach (var QuestionID in formCollection.AllKeys)
                {
                    var Answer = formCollection[QuestionID];

                    TypecastedQuesID = Convert.ToInt32(QuestionID);
                    TypecastedAnsID = Convert.ToInt32(Answer);

                    studentUtil.InsertAnswer(ResultID, TypecastedQuesID, TypecastedAnsID);
                }

                return RedirectToAction("Result", new { ID = ResultID });
            }
            else
            {
                ViewBag.ErrorMessage = "An error occured while loading exam!<br>Kindly try again later";
                return View("Error");
            }
        }

        public ActionResult Results()
        {
            StudentUtil studentUtil = new StudentUtil();

            return View(studentUtil.AllResults(Convert.ToInt32(Session["StudentID"])));
        }
        
        public ActionResult Result(int ID)
        {
            StudentUtil studentUtil = new StudentUtil();
            AdminUtil adminUtil = new AdminUtil();
            ResultBundle resultBundle = new ResultBundle();

            resultBundle.Result = studentUtil.GetResultByID(ID);
            resultBundle.Exam = adminUtil.GetExamByID(resultBundle.Result.ExamID);
            resultBundle.AllQuestion = studentUtil.ExamQuestionsWithResult(resultBundle.Result.ExamID, ID);

            return View(resultBundle);
        }

        public ActionResult Settings()
        {
            AccountUtil account = new AccountUtil();
            return View(account.GetUserByID(Convert.ToInt32(Session["StudentID"])));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(User user)
        {
            AccountUtil account = new AccountUtil();
            user.ID = Convert.ToInt32(Session["StudentID"]);
            if (account.UpdateUser(user))
            {
                Session["Notification"] = 1;
            }
            else
            {
                Session["Notification"] = 2;
            }
            return RedirectToAction("Settings");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(FormCollection formCollection)
        {
            string OldPassword = Convert.ToString(formCollection["OldPassword"]);
            string NewPassword = Convert.ToString(formCollection["NewPassword"]);

            AccountUtil account = new AccountUtil();
            User user = account.GetUserByID(Convert.ToInt32(Session["StudentID"]));

            if (user.Password == OldPassword)
            {
                if (account.UpdateUserPassword(NewPassword, user.ID))
                {
                    Session["Notification"] = 3;
                }
                else
                {
                    Session["Notification"] = 4;
                }
            }
            else
            {
                Session["Notification"] = 5;
            }

            return RedirectToAction("Settings");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}