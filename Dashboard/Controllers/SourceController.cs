using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.Models;
using QuestionsSYS.Context;

namespace QuestionsSYS.Controllers
{
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
    }
}