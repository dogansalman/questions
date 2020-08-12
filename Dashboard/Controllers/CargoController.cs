using System;
using System.Linq;
using System.Web.Mvc;
using QuestionsSYS.Models;
using QuestionsSYS.Context;

namespace QuestionsSYS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CargoController : Controller
    {
        DatabaseContexts db = new DatabaseContexts();
        // GET: Cargo
        public ActionResult Index()
        {
            return View(db.cargo.ToList());
        }


        [HttpPost]
        public ActionResult Add([Bind(Include = "cargo_company")] Cargo model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Cargo", new { success = "failed" });

            try
            {

                Cargo s = new Cargo { cargo_company = model.cargo_company };
                db.cargo.Add(s);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Cargo", new { success = "failed" });
            }

            return RedirectToAction("Index", "Cargo", new { success = "ok" });

        }
        [HttpPost]
        public ActionResult Update([Bind(Include = "id,cargo_company")] Cargo model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Cargo", new { success = "failed" });
            try
            {
                Cargo s = db.cargo.Where(src => src.id == model.id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                s.cargo_company = model.cargo_company;
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
                Cargo s = db.cargo.Where(src => src.id == id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                db.cargo.Remove(s);
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