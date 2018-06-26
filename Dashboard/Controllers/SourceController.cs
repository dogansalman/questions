using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.Models;
using QuestionsSYS.Context;

namespace QuestionsSYS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SourceController : Controller
    {

        DatabaseContexts db = new DatabaseContexts();
        // GET: Soruce
        public ActionResult Index()
        {
            return View(db.sources.ToList());
        }
        
        [HttpPost]
        public ActionResult Add([Bind(Include = "name")] Soruce model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Source", new { success = "failed" });

            try
            {
                Soruce s = new Soruce { name = model.name };
                db.sources.Add(s);
                db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                if (!ModelState.IsValid) return RedirectToAction("Index", "Source", new { success = "failed" });
            }

            return RedirectToAction("Index", "Source", new { success = "ok" });

        }
        [HttpPost]
        public ActionResult Update([Bind(Include = "id,name")] Soruce model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Source", new { success = "failed" });
            try
            {
                Soruce s = db.sources.Where(src => src.id == model.id).FirstOrDefault();
                if(s == null) return new HttpStatusCodeResult(404);
                s.name = model.name;
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
                Soruce s = db.sources.Where(src => src.id == id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                db.sources.Remove(s);
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