using System;
using System.Linq;
using System.Web.Mvc;
using QuestionsSYS.Models;
using QuestionsSYS.Context;

namespace QuestionsSYS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JobsController : Controller
    {

        DatabaseContexts db = new DatabaseContexts();
        // GET: Jobs
        public ActionResult Index()
        {
            return View(db.jobs.ToList());
        }
        
        [HttpPost]
        public ActionResult Add([Bind(Include = "job_name")] Jobs model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Jobs", new { success = "failed" });

            try
            {
                
                Jobs s = new Jobs{ job_name = model.job_name };
                db.jobs.Add(s);
                db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Jobs", new { success = "failed" });
            }

            return RedirectToAction("Index", "Jobs", new { success = "ok" });

        }
        [HttpPost]
        public ActionResult Update([Bind(Include = "id,job_name")] Jobs model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Jobs", new { success = "failed" });
            try
            {
                Jobs s = db.jobs.Where(src => src.id == model.id).FirstOrDefault();
                if(s == null) return new HttpStatusCodeResult(404);
                s.job_name = model.job_name;
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500);
                throw;
            }           
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            
            try
            {
                Jobs s = db.jobs.Where(src => src.id == id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                db.jobs.Remove(s);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500);
                throw;
            }
        }
    }
}