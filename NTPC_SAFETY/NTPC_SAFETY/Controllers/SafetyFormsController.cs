using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NTPC_SAFETY.Models;
using System.Web.UI;

namespace NTPC_SAFETY.Controllers
{
    public class SafetyFormsController : Controller
    {
        private NTPCProjectEntities1 db = new NTPCProjectEntities1();

        // GET: SafetyForms
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.ProjNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date" ;
            var safetyForm = from s in db.SafetyForms
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                safetyForm = safetyForm.Where(s => s.Project.ProjName.Contains(searchString));
                                       
            }
            switch (sortOrder)
            {
                case "name_desc":
                    safetyForm = safetyForm.OrderByDescending(s => s.ProjCode);
                    break;
                case "Date":
                    safetyForm = safetyForm.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    safetyForm = safetyForm.OrderByDescending(s => s.Date);
                    break;
                default:
                    safetyForm = safetyForm.OrderBy(s => s.ProjCode);
                    break;
            }

            return View(safetyForm.ToList());
        }
        public ActionResult LoggedIn()
        {
            return View();
        }
        // GET: SafetyForms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyForm safetyForm = db.SafetyForms.Find(id);
            if (safetyForm == null)
            {
                return HttpNotFound();
            }
            return View(safetyForm);
        }

        //GET: SafetyForms/Audit/3104
        public ActionResult Audit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (!Session["Admin"].ToString().Equals("True"))
            {
                return RedirectToAction("Sorry");
            }

            else if (Session["Admin"].ToString().Equals("True"))
            {

                SafetyForm safetyForm = db.SafetyForms.Find(id);

                if (safetyForm == null)
                {
                    return HttpNotFound();
                }


                return View(safetyForm);
            }
            return View();
        }


        [HttpPost, ActionName("Audit")]
        [ValidateAntiForgeryToken]
        public ActionResult AuditConfirmed(int id)
        {
            SafetyForm safetyForm = db.SafetyForms.Find(id);
            
                if (safetyForm != null)
                {

                    safetyForm.Lock = true;
                    db.SaveChanges();

                }
                          
            return RedirectToAction("Index");
        }
       
        public ActionResult Sorry()
        {

            return View();
        }

        // GET: SafetyForms/Create
        public ActionResult Create()
        {


            //ViewBag.ProjCode = new SelectList(db.Projects, "ProjCode", "ProjName

            if(Session["EmpID"] == null)
            {
                return RedirectToAction("LoginToContinue");
            }
            
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjCode,Date,SafetyTip,TrainingsToTrainers,ID,RiskGovernance,TrainingToContractor,TrainingToNTPCEng,TrainingToHods,SafetyPolicyDisplay,StartOfMeetingsWithSafety,DistributionOfPPES,EntryWithPPES,SafetyControlRoomFunc,ScreeningOfSafetyFilms,OverHaulsUnderDUPONT,SafetyContractsMgmt,RevisedSafetyProvisions,SafetyProcessMgmt,InternalAudits,IntenalAuditsActionTaken,InternalAuditSchedule,ExternalAudits,ExternalAuditsActionTaken,ExternalAuditSchedule,Compliance,InternalMOU,ArticleOfTheMonth,SafetyInBusinessModel,ReplaceOldConcreteSlabs,ProtectionOfStructures,DividingPlantIntoAreas,CheckListForEngineers,MockDrillsStatus,FactoryOccupierDeclarationStatus,ATROnDirectors")] SafetyForm safetyForm)
        {
            safetyForm.ProjCode = Session["ProjCode"].ToString();
            
            if (ModelState.IsValid)
            {
               
                db.SafetyForms.Add(safetyForm);

                Report report = db.Reports.SingleOrDefault(r => r.ProjCode == safetyForm.ProjCode);
                if (report != null)
                {
                    
                        DateTime date1 = report.LastSubmitDate;
                    
                    DateTime date2 = safetyForm.Date;
                    if (date2.CompareTo(date1) > 0)
                    {
                        report.LastSubmitDate = safetyForm.Date;
                    }


                    if (report.LastSubmitDate.Month == System.DateTime.Now.Month && report.LastSubmitDate.Year == System.DateTime.Now.Year)
                    {
                        report.IsThisMonthReport = true;
                        ViewBag.ReportUpdated = "Report Updated! ";
                    }
                    else
                        report.IsThisMonthReport = false;

                }


                db.SaveChanges();
                
                return RedirectToAction("Index");
                
            }
            //ViewBag.ProjCode = new SelectList(db.Projects, "ProjCode", "ProjName", safetyForm.ProjCode);

            return View(safetyForm);
        }

        // GET: SafetyForms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyForm safetyForm = db.SafetyForms.Find(id);
            //safetyForm.ProjCode = Session["ProjCode"].ToString();
            
            if (safetyForm.Lock == true)
            {
                return RedirectToAction("Locked");
            }
            if (safetyForm == null)
            {
                return HttpNotFound();
            }
            
            return View(safetyForm);
        }

        // POST: SafetyForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjCode,Date,SafetyTip,TrainingsToTrainers,ID,RiskGovernance,TrainingToContractor,TrainingToNTPCEng,TrainingToHods,SafetyPolicyDisplay,StartOfMeetingsWithSafety,DistributionOfPPES,EntryWithPPES,SafetyControlRoomFunc,ScreeningOfSafetyFilms,OverHaulsUnderDUPONT,SafetyContractsMgmt,RevisedSafetyProvisions,SafetyProcessMgmt,InternalAudits,IntenalAuditsActionTaken,InternalAuditSchedule,ExternalAudits,ExternalAuditsActionTaken,ExternalAuditSchedule,Compliance,InternalMOU,ArticleOfTheMonth,SafetyInBusinessModel,ReplaceOldConcreteSlabs,ProtectionOfStructures,DividingPlantIntoAreas,CheckListForEngineers,MockDrillsStatus,FactoryOccupierDeclarationStatus,CompensationTracking,UpdateOnRehab,ATROnDirectors")] SafetyForm safetyForm)
        {
          
            if (ModelState.IsValid)
            {
                
               
                db.Entry(safetyForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ProjCode = new SelectList(db.Projects, "ProjCode", "ProjName", safetyForm.ProjCode);
            return View(safetyForm);
        }

        // GET: SafetyForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SafetyForm safetyForm = db.SafetyForms.Find(id);
           
            

            if (safetyForm.Lock==true)
            {
                return RedirectToAction("Locked");
            }

           
            if (safetyForm == null)
            {
                return HttpNotFound();
            }
            return View(safetyForm);
        }

        // POST: SafetyForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SafetyForm safetyForm = db.SafetyForms.Find(id);           
            db.SafetyForms.Remove(safetyForm);
            db.SaveChanges();

            //Report report = db.Reports.SingleOrDefault(r => r.ProjCode == safetyForm.ProjCode);
            //if (safetyForm.Date == report.LastSubmitDate)
            //{
              //  report.LastSubmitDate = db.SafetyForms.
            //}


            return RedirectToAction("Index");
        }
        public ActionResult Locked()

        {
            return View();
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
