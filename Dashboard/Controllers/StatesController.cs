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
    public class StatesController : Controller
    {

        DatabaseContexts db = new DatabaseContexts();
        // GET: States
        public ActionResult Index()
        {
            return View(db.states.ToList());
        }
        
        [HttpPost]
        public ActionResult Add([Bind(Include = "name")] States model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "States", new { success = "failed" });

            try
            {
                
                States s = new States { name = model.name };
                db.states.Add(s);
                db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                if (!ModelState.IsValid) return RedirectToAction("Index", "States", new { success = "failed" });
            }

            return RedirectToAction("Index", "States", new { success = "ok" });

        }
        [HttpPost]
        public ActionResult Update([Bind(Include = "id,name")] States model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "States", new { success = "failed" });
            try
            {
                States s = db.states.Where(src => src.id == model.id).FirstOrDefault();
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
                States s = db.states.Where(src => src.id == id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                db.states.Remove(s);
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