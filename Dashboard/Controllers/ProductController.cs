using QuestionsSYS.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.Models;
using System.Security.Permissions;

namespace QuestionsSYS.Controllers
{

    public class ProductName
    {
        public string product_name { get; set; }
    }
    public class ProductController : Controller
    {
        DatabaseContexts db = new DatabaseContexts();
        // GET: Product
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.products.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Add([Bind(Include = "product_name,price")] Product model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);
            
            try
            {
                Product product = new Product { price = model.price, product_name = model.product_name };
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index", "Product", new { success = product.id});

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(302);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {

            try
            {
                Product s = db.products.Where(src => src.id == id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                db.products.Remove(s);
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
        [Authorize(Roles = "Admin")]
        public ActionResult Update([Bind(Include = "id,product_name, price")] Product model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);
            try
            {
                Product s = db.products.Where(src => src.id == model.id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                s.product_name = model.product_name;
                s.price = model.price;
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
        [Authorize]
        public ActionResult Price([Bind(Include = "product_name")] ProductName model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);
            try
            {
                Product s = db.products.Where(src => src.product_name == model.product_name).FirstOrDefault();
                return Json(s);
               
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500);
                throw;
            }
        }

    }
}