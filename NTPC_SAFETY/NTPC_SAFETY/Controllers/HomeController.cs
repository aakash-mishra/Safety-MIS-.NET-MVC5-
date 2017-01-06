using NTPC_SAFETY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTPC_SAFETY.Controllers
{
    public class HomeController : Controller
    {
        private NTPCProjectEntities1 db = new NTPCProjectEntities1();
        public ActionResult Index()
        {

           
            return View();
        }

        [HttpPost]

        public ActionResult Index(User user)
        {

            var usr = db.Users.Where(u => u.EmpID == user.EmpID && u.Password==user.Password).FirstOrDefault();
            if (usr != null)
            {

                Session["EmpID"] = usr.EmpID.ToString();
                Session["ProjName"] = usr.ProjName.ToString();
                Session["Name"] = usr.EmpName.ToString();
                Session["ProjCode"] = usr.ProjCode.ToString();
                Session["Admin"] = usr.Admin.ToString();
                return RedirectToAction("LoggedIn", "SafetyForms");
            }

            else
            {

                ModelState.AddModelError("", "Invalid Employee number or Password");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("LoggedOut");


            //return View();
        }

        public ActionResult LoggedOut()
        {
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}