using OnlineExaminationSystem.Models;
using OnlineExaminationSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [IsAuthorized]
        [Route("AdminLogin")]
        public ActionResult AdminLogin()
        {
            Session["AdminID"] = 1;
            Session["Name"] = "Mohammed Saad";
            return RedirectToAction("Index", "Admin");

            //return View();
        }

        [IsAuthorized]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AdminLogin")]
        public ActionResult AdminLogin(Admin admin)
        {
            AccountUtil accountUtil = new AccountUtil();

            if (accountUtil.AdminLogin(admin.Email, admin.Password))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.ErrorMessage = "Incorrect email or password";
                return View();
            }
                
        }

        [IsAuthorized]
        [Route("Login")]
        public ActionResult Login()
        {
            Session["StudentID"] = 1;
            Session["Name"] = "Mohammed Saad";
            return RedirectToAction("Index", "Student");

            //int Notification = 0;
            //try
            //{
            //    Notification = Convert.ToInt32(Session["Notification"]);
            //}
            //catch
            //{ }

            //if (Notification == 1)
            //{
            //    ViewBag.SuccessMessage = "Account successfully created!<br>Kindly login to access dashboard";
            //    Session["Notification"] = null;
            //}
            //return View();
        }

        [IsAuthorized]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public ActionResult Login(User user)
        {
            AccountUtil accountUtil = new AccountUtil();
            int result = accountUtil.UserLogin(user.Email, user.Password);
            if (result == 0) 
            {
                ViewBag.ErrorMessage = "Incorrect email or password";
            }
            else if (result == 1)
            {
                ViewBag.ErrorMessage = "Your account is blocked! Kindly contact admin " + AppInfo.OwnerEmail;
            }
            else if (result == 2)
            {
                return RedirectToAction("Index", "Student");
            }
            return View();
        }

        [IsAuthorized]
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        
        [IsAuthorized]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public ActionResult Register(User user)
        {
            user.DateTime = DateTime.Now;
            user.Status = 1;

            AccountUtil accountUtil = new AccountUtil();
            if (accountUtil.AddUser(user))
            {
                Session["Notification"] = 1;
                return RedirectToAction("Login");
            }
            return View();
        }

        [Route("ContactUs")]
        public ActionResult Contact()
        {
            int Notification = 0;
            try
            {
                Notification = Convert.ToInt32(Session["Notification"]);
            }
            catch
            { }

            if (Notification == 1)
            {
                ViewBag.SuccessMessage = true;
                Session["Notification"] = null;
            }
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ContactUs")]
        public ActionResult Contact(ContactUs contactUs)
        {
            contactUs.DateTime = DateTime.Now;
            contactUs.Seen = 0;
            LandingPageUtil landingPageUtil = new LandingPageUtil();
            if (landingPageUtil.AddEnquiry(contactUs))
            {
                Session["Notification"] = 1;
                return RedirectToAction("Contact");
            }
            else
            {
                ViewBag.ErrorMessage = true;
            }       
            return View();
        }

    }
}