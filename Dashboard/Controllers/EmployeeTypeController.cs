using QuestionsSYS.Context;
using QuestionsSYS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestionsSYS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeTypeController : Controller
    {
        DatabaseContexts db = new DatabaseContexts();
        // GET: EmployeeType
        public ActionResult Index()
        {
            return View(db.employee_type.ToList());
        }

        [HttpPost]
        public ActionResult Add([Bind(Include = "type_name")] EmployeeType model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "EmployeeType", new { success = "failed" });

            try
            {

                EmployeeType s = new EmployeeType { type_name = model.type_name };
                db.employee_type.Add(s);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "EmployeeType", new { success = "failed" });
            }

            return RedirectToAction("Index", "EmployeeType", new { success = "ok" });

        }
        [HttpPost]
        public ActionResult Update([Bind(Include = "id,type_name")] EmployeeType model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "EmployeeType", new { success = "failed" });
            try
            {
                EmployeeType s = db.employee_type.Where(src => src.id == model.id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                s.type_name = model.type_name;
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
                EmployeeType s = db.employee_type.Where(src => src.id == id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                db.employee_type.Remove(s);
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