using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NTPC_SAFETY.Models;

namespace NTPC_SAFETY.Controllers
{
    public class ReportsController : Controller
    {
        private NTPCProjectEntities1 db = new NTPCProjectEntities1();

        // GET: Reports
        public ActionResult Index(int? id)
        {

            var reports = db.Reports.Include(r => r.Project);

            foreach(Report rep in reports)
            {
                if (rep.LastSubmitDate.Month == System.DateTime.Now.Month && rep.LastSubmitDate.Year == System.DateTime.Now.Year)
                {
                    rep.IsThisMonthReport = true;
                    ViewBag.ReportUpdated = "Report Updated! ";
                }
                else
                    rep.IsThisMonthReport = false;
            }

       

            return View(reports.ToList());
        }
        
       
        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
