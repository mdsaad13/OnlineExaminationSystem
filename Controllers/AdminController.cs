using OnlineExaminationSystem.DAL;
using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        public AdminController()
        {
            ViewBag.DashBoardType = "Admin";
            General general = new General();
            int NewEnqs = general.CountByArgs("enquiry", "seen = 0");
            if (NewEnqs > 0)
            {
                ViewBag.PendingEnqToSee = $"<span class=\"badge badge-danger\">{NewEnqs} new</span>";
            }
            else
            {
                ViewBag.PendingEnqToSee = null;
            }
            
        }

        public ActionResult Index()
        {
            General general = new General();
            ViewBag.UsersCount = general.Count("users");
            ViewBag.ExamsCount = general.Count("exams");
            ViewBag.EnqsCount = general.Count("enquiry");

            return View();
        }
        
        public ActionResult ContactUs()
        {
            AdminUtil adminUtil = new AdminUtil();
            List<ContactUs> Data = adminUtil.AllEnquiries();
            adminUtil.UpdateEnqToSeen();
            return View(Data);
        }

        /* User operations starts here */
        public ActionResult Users()
        {
            AccountUtil accountUtil = new AccountUtil();
            return View(accountUtil.AllUsers());
        } 
        
        public ActionResult UsersDetails(int ID)
        {
            StudentUtil studentUtil = new StudentUtil();
            return View(studentUtil.AllResults(ID));
        }
        
        public ActionResult UsersResultDetails(int ID)
        {
            StudentUtil studentUtil = new StudentUtil();
            AdminUtil adminUtil = new AdminUtil();
            ResultBundle resultBundle = new ResultBundle();

            resultBundle.Result = studentUtil.GetResultByID(ID);
            resultBundle.Exam = adminUtil.GetExamByID(resultBundle.Result.ExamID);
            resultBundle.AllQuestion = studentUtil.ExamQuestionsWithResult(resultBundle.Result.ExamID, ID);

            return View(resultBundle);
        }

        public ActionResult AddUser()
        {
            ViewBag.ShowPassword = true;
            ViewBag.Title = "Add Users";
            return View("UserForm");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(User user)
        {
            user.DateTime = DateTime.Now;
            user.Status = 1;
            AccountUtil accountUtil = new AccountUtil();
            if (accountUtil.AddUser(user))
            {
                Session["Notification"] = 1;
            }
            else
            {
                Session["Notification"] = 2;
            }
            return RedirectToAction("Users");
        }
        
        public ActionResult EditUser(int ID)
        {
            ViewBag.Title = "Edit Users";
            ViewBag.ShowStatus = true;
            AccountUtil accountUtil = new AccountUtil();
            User user = accountUtil.GetUserByID(ID);
            return View("UserForm", user);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            AccountUtil accountUtil = new AccountUtil();
            if (accountUtil.UpdateUser(user))
            {
                Session["Notification"] = 3;
            }
            else
            {
                Session["Notification"] = 4;
            }
            return RedirectToAction("Users");
        }
        /* User operations ends here */

        /* Subjects operations starts here */
        public ActionResult Subjects()
        {
            AdminUtil adminUtil = new AdminUtil();
            return View(adminUtil.AllSubjects());
        }

        public ActionResult AddSubject()
        {
            ViewBag.Title = "Add Subject";
            return View("SubjectForm");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubject(Subjects subjects)
        {
            AdminUtil adminUtil = new AdminUtil();
            if (adminUtil.AddSubject(subjects))
            {
                Session["Notification"] = 1;
            }
            else
            {
                Session["Notification"] = 2;
            }
            return RedirectToAction("Subjects");
        }
        
        public ActionResult EditSubject(int ID)
        {
            ViewBag.Title = "Edit Subject";
            AdminUtil adminUtil = new AdminUtil();
            return View("SubjectForm", adminUtil.GetSubjectByID(ID));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubject(Subjects subjects)
        {
            AdminUtil adminUtil = new AdminUtil();
            if (adminUtil.UpdateSubject(subjects))
            {
                Session["Notification"] = 3;
            }
            else
            {
                Session["Notification"] = 4;
            }
            return RedirectToAction("Subjects");
        }
        /* Subjects operations ends here */

        /* Exams operations starts here */
        public ActionResult Exams()
        {
            AdminUtil adminUtil = new AdminUtil();
            return View(adminUtil.AllExams());
        }

        public ActionResult AddExam()
        {
            AdminUtil adminUtil = new AdminUtil();
            ViewBag.Title = "Add Exam";
            ViewBag.AllSubjects = new SelectList(adminUtil.AllSubjects(), "id", "name");
            return View("ExamForm");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExam(Exam exam)
        {
            exam.Status = 0;
            exam.DateTime = DateTime.Now;
            AdminUtil adminUtil = new AdminUtil();
            if (adminUtil.AddExam(exam))
            {
                Session["Notification"] = 1;
            }
            else
            {
                Session["Notification"] = 2;
            }
            return RedirectToAction("Exams");
        }

        public ActionResult EditExam(int ID)
        {
            ViewBag.Title = "Edit Exam";
            AdminUtil adminUtil = new AdminUtil();
            ViewBag.AllSubjects = new SelectList(adminUtil.AllSubjects(), "id", "name");
            return View("ExamForm", adminUtil.GetExamByID(ID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExam(Exam exam)
        {
            AdminUtil adminUtil = new AdminUtil();
            if (adminUtil.UpdateExam(exam))
            {
                Session["Notification"] = 5;
            }
            else
            {
                Session["Notification"] = 6;
            }
            return RedirectToAction("ViewExam", new { id = exam.ID });
        }
        
        
        public ActionResult ViewExam(int ID)
        {
            ViewBag.Title = "View Exam";
            AdminUtil adminUtil = new AdminUtil();
            ExamBundle examBundle = new ExamBundle();

            examBundle.Exam = adminUtil.GetExamByID(ID);
            examBundle.AllQuestion = adminUtil.AllExamQuestions(ID);

            return View(examBundle);
        }

        [Route("Admin/ViewExam/{ID}/AddQuestion")]
        public ActionResult AddQuestion(int ID)
        {
            ViewBag.FormSubmit = "AddQuestion";
            ViewBag.Title = "Add Question";
            Question question = new Question();
            question.ExamID = ID;
            question.Answer = 1;
            question.Marks = 2;
            return View("QuestionForm", question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(Question question)
        {
            AdminUtil adminUtil = new AdminUtil();
            if (adminUtil.AddExamQuestion(question))
            {
                Session["Notification"] = 1;
            }
            else
            {
                Session["Notification"] = 2;
            }
            return RedirectToAction("ViewExam", new { id = question.ExamID });
        }

        [Route("Admin/ViewExam/{ExamID}/EditQuestion/{ID}")]
        public ActionResult UpdateQuestion(int ID, int ExamID)
        {
            ViewBag.FormSubmit = "UpdateQuestion";
            ViewBag.Title = "Update Question";
            AdminUtil adminUtil = new AdminUtil();
            return View("QuestionForm", adminUtil.GetQuestionByID(ID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateQuestion(Question question)
        {
            AdminUtil adminUtil = new AdminUtil();
            if (adminUtil.UpdateExamQuestion(question))
            {
                Session["Notification"] = 3;
            }
            else
            {
                Session["Notification"] = 4;
            }
            return RedirectToAction("ViewExam", new { id = question.ExamID });
        }

        public ActionResult DeleteQuestion(int ID, int ExamID)
        {
            AdminUtil adminUtil = new AdminUtil();
            adminUtil.DeleteQuestion(ID);
            Session["Notification"] = 7;
            return RedirectToAction("ViewExam", new { id = ExamID });
        }

        public ActionResult EnableDisableExam(int ID, int Status)
        {
            AdminUtil adminUtil = new AdminUtil();
            if (adminUtil.UpdateExamStatus(ID, Status))
            {
                Session["Notification"] = 8;
            }
            else
            {
                Session["Notification"] = 9;
            }
            return RedirectToAction("ViewExam", new { id = ID });
        }

        /* Exams operations ends here */


        public ActionResult Settings()
        {
            AccountUtil account = new AccountUtil();
            return View(account.GetAdminByID(Convert.ToInt32(Session["AdminID"])));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(Admin user)
        {
            AccountUtil account = new AccountUtil();
            user.ID = Convert.ToInt32(Session["AdminID"]);
            if (account.UpdateAdmin(user))
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
            Admin user = account.GetAdminByID(Convert.ToInt32(Session["AdminID"]));

            if (user.Password == OldPassword)
            {
                if (account.UpdateAdminPassword(NewPassword, user.ID))
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

        [HttpGet]
        [Route("Admin/Logout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}