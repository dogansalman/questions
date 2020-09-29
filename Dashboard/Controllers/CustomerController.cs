using QuestionsSYS.Context;
using QuestionsSYS.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QuestionsSYS.Models;
using QuestionsSYS.ModelViews;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Web.Routing;
using System.Security.Claims;

namespace QuestionsSYS.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        DatabaseContexts db = new DatabaseContexts();

        // GET: Customer
        public ActionResult Index()
        {
            string user_id = User.Identity.GetUserId();


            var customers = (
               from t in db.customers
               join c in db.cities on t.city equals c.id
               join s in db.towns on t.town equals s.id
               select new CustomerList
               {
                   id = t.id,
                   added = t.added,
                   name = t.name,
                   lastname = t.lastname,
                   address = t.address,
                   city = c.city_name,
                   phone = t.phone,
                   town = s.town_name,
                   user_id = t.user_id
               }
               );

            if(User.IsInRole("User"))
            {
                customers = customers.Where(t => t.user_id == user_id);
            }
        
            return View(customers);

        }
       
        public ActionResult New()
        {
            ViewBag.cities = db.cities.ToList();
            ViewBag.jobs = db.jobs.ToList();
           

            return View();
        }


        [Route("History/{*id}")]
        public ActionResult History(int id)
        {
            string user_id = User.Identity.GetUserId();
            Customer customer = db.customers.Where(qu => qu.id == id).FirstOrDefault();
            
            if (customer == null) return new HttpStatusCodeResult(404, "Not found");
            if (!User.IsInRole("Admin") && customer.user_id != user_id)
            {
                return new HttpStatusCodeResult(403, "Forbidden!");
            }
            ViewBag.customer = customer;
            ViewBag.customer_id = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddHistory([Bind(Include = "description, date, customer_id")] CustomerHistory model)
        {
            try
            {
                string user_id = User.Identity.GetUserId();
                Customer customer = db.customers.Where(qu => qu.id == model.customer_id && qu.user_id == user_id).FirstOrDefault();
                if(customer == null) return new HttpStatusCodeResult(500, "Not permit to customer");

                CustomerHistory q = new CustomerHistory
                {
                    description = model.description,
                    date = DateTime.Now,
                    customer_id = model.customer_id
                };
                db.customer_history.Add(q);
                db.SaveChanges();

                return new HttpStatusCodeResult(200);

            }
            catch (Exception exs)
            {
                return new HttpStatusCodeResult(500, exs.Message.ToString());
            }

        }
        [HttpPost]
        public ActionResult UpdateHistory([Bind(Include = "description, id")] CustomerHistory model)
        {
           
            try
            {
                string user_id = User.Identity.GetUserId();
                CustomerHistory history = db.customer_history.Where(qu => qu.id == model.id).FirstOrDefault();
                if(history == null) return new HttpStatusCodeResult(404, "Not Found History");

                Customer customer = db.customers.Where(qu => qu.id == history.customer_id).FirstOrDefault();

                if (!User.IsInRole("Admin") && customer.user_id != user_id)
                {
                    return new HttpStatusCodeResult(403, "Forbidden!");
                }


             
                history.description = model.description;
                db.SaveChanges();
                return new HttpStatusCodeResult(200);

            }
            catch (Exception exs)
            {
                return new HttpStatusCodeResult(500, exs.Message.ToString());
            }

        }
       

        [Route("HistoryDetail/{*id}")]
        public ActionResult HistoryDetail(int id)
        {
          

            string user_id = User.Identity.GetUserId();
            CustomerHistory history = db.customer_history.Where(qu => qu.id == id).FirstOrDefault();

            Customer customer = db.customers.Where(qu => qu.id == history.customer_id).FirstOrDefault();

            if (!User.IsInRole("Admin"))
            {
                if(customer.user_id != user_id)
                {
                    return new HttpStatusCodeResult(403, "Forbidden!");
                }
                    
            }
            ViewBag.customer = customer;
               

            return View(history);
        }

        [HttpPost]
        public ActionResult DeleteHistory(int? id)
        {
            string user_id = User.Identity.GetUserId();
            try
            {
                CustomerHistory s = db.customer_history.Where(src => src.id == id).FirstOrDefault();
                Customer c = db.customers.Where(f => f.id == s.customer_id).FirstOrDefault();

                if (!User.IsInRole("Admin") && c.user_id != user_id)
                {
                    return new HttpStatusCodeResult(403, "Forbidden!");
                }

                if (s == null) return new HttpStatusCodeResult(404);
                db.customer_history.Remove(s);
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
        public ActionResult Town(int id)
        {
            return Json(db.towns.Where(c => c.city_id == id).ToList());
        }

        [HttpPost]
        public ActionResult Add([Bind(Include = "name, lastname, address, city, town, phone, birth_year, job")] Customer model)
        {
            try
            {
                
                string user_id = User.Identity.GetUserId();
                var u = db.Users.Where(p => p.Id == user_id).FirstOrDefault();


                Customer q = new Customer {
                    name = model.name,
                    lastname = model.lastname,
                    phone = model.phone,
                    address = model.address,
                    city = model.city,
                    town = model.town,
                    job = model.job,
                    user_id = user_id,
                    birth_year = model.birth_year,
                    user_fullname = u.Name + " " + u.Surname

                };
                db.customers.Add(q);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);

            }
            catch (Exception exs)
            {
                return new HttpStatusCodeResult(500, exs.Message.ToString());
            }
           
        }
        [HttpPost]
        [Authorize]
        public ActionResult Customer(int id)
        {
            Customer c = db.customers.Where(cu => cu.id == id).FirstOrDefault();
            if (c == null) Json(null);
            return Json(c);
        }
        public ActionResult Detail(int? id)
        {
            ViewBag.cities = db.cities.ToList();
            ViewBag.jobs = db.jobs.ToList();
            ViewBag.history = db.customer_history.Where(c => c.customer_id == id).ToList();
            
            var user_id = User.Identity.GetUserId();

            if (!User.IsInRole("Admin"))
            {
                Customer q = db.customers.Where(qu => qu.id == id && qu.user_id == user_id).FirstOrDefault();
                if (q == null) return new HttpStatusCodeResult(404);
                ViewBag.towns = db.towns.Where(t => t.city_id == q.city).ToList();
                return View(q);
            } else {
                Customer q = db.customers.Where(qu => qu.id == id).FirstOrDefault();
                if (q == null) return new HttpStatusCodeResult(404);
                ViewBag.towns = db.towns.Where(t => t.city_id == q.city).ToList();
                return View(q);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int? id)
        {

            // order validate
            var is_exist_order = db.orders.Where(o => o.customer_id == id).FirstOrDefault();
            if(is_exist_order != null)
            {
                return new HttpStatusCodeResult(500);
            }

            try
            {
                Customer s = db.customers.Where(q => q.id == id).FirstOrDefault();
                // TODO siparis vb de kullanılıyorsa silinmemeli.
                if (s == null) return new HttpStatusCodeResult(404);
                db.customers.Remove(s);
                db.SaveChanges();
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Customer", action = "Index", Id = id, success = "failed_remove" }));
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500);
                throw;
            }
        }


        [HttpPost]
        public ActionResult Update(int? id, [Bind(Include = "name, lastname, address, city, town, phone, birth_year, job")] Customer model)
        {
            var user_id = User.Identity.GetUserId();
            Customer q = db.customers.Where(qu => qu.id == id).FirstOrDefault();

            if (q == null) return new HttpStatusCodeResult(404);

            if (!User.IsInRole("Admin") && q.user_id != user_id )
            {
                return new HttpStatusCodeResult(403);
            }


            q.name = model.name;
            q.lastname = model.lastname;
            q.phone = model.phone;
            q.address = model.address;
            q.city = model.city;
            q.town = model.town;
            q.job = model.job;
            q.birth_year = model.birth_year;

            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }
    }
}
