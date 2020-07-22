using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QuestionsSYS.Context;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web.Routing;
using QuestionsSYS.Models;
using System.Security.Cryptography;
using QuestionsSYS.ModelViews;

namespace QuestionsSYS.Controllers
{
 
    public class OrderController : Controller
    {

       
        DatabaseContexts db = new DatabaseContexts();
        public ActionResult Index()
        {
            var user_id = User.Identity.GetUserId();
            var orders = (
                from o in db.orders
                join c in db.cities on o.city equals c.id
                join t in db.towns on o.town equals t.id
                select new OrderList
                {
                    id = o.id,
                    added = o.added,
                    city = c.city_name,
                    town = t.town_name,
                    lastname = o.last_name,
                    name = o.name,
                    order_state = o.state,
                    phone = o.phone,
                    total = o.total,
                    user_id = o.user_id
                }
                );
            if(!User.IsInRole("Admin"))
            {
                orders.Where(o => o.user_id == user_id);
            }

            return View(orders.ToList());
        }


        [Authorize]
        [HttpPost]
        public ActionResult ProductsAdd(int id, [Bind(Include = "id, product_name, qty, price")] QuestionsSYS.ModelViews.Product model)
        {
            var user_id = User.Identity.GetUserId();
            var order = db.orders;
            order.Where(o => o.id == id);
            if(!User.IsInRole("Admin"))
            {
                order.Where(o => o.user_id == user_id);
            };

            var order_detail = order.FirstOrDefault();
            if(order_detail != null)
            {
                OrderProduct op = new OrderProduct
                {
                    price = model.price,
                    product_name = model.product_name,
                    qty = model.qty,
                    orderId = id
                };
                db.order_products.Add(op);
                db.SaveChanges();
                
                return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = id, success = "ok" }));
            }
            return new HttpStatusCodeResult(500);

        }
        [Authorize]
        public ActionResult Detail(int id)
        {
            ViewBag.products = db.products.ToList();
            var user_id = User.Identity.GetUserId();
            ViewBag.task_id = id;
            var customers = (
               from t in db.customers
               join c in db.cities on t.city equals c.id
               join d in db.towns on t.town equals d.id
               select new CustomerList
               {
                   name = t.name,
                   lastname = t.lastname,
                   city = c.city_name,
                   town = d.town_name
               }
               ).ToList();
            
            if (!User.IsInRole("Admin"))
            {
                customers.Where(c => c.user_id == user_id);
            }
            ViewBag.customers = customers.ToList();
            ViewBag.cities = db.cities.ToList();
            


            var order_ = db.orders.Where(s => s.id == id);
            if (!User.IsInRole("Admin"))
            {
                order_.Where(c => c.user_id == user_id);
            }
            Order o = order_.FirstOrDefault();

            ViewBag.towns = db.towns.Where(t => t.city_id == o.city).ToList();

            return View(o);
        }


        [Authorize]
        public ActionResult New(string id)
        {
            var user_id = User.Identity.GetUserId();
            ViewBag.task_id = id;
            var customers = (
               from t in db.customers
               join c in db.cities on t.city equals c.id
               join d in db.towns on t.town equals d.id
               select new CustomerList
               {
                   name = t.name,
                   lastname = t.lastname,
                   city = c.city_name,
                   town = d.town_name
               }
               ).ToList();

            if (!User.IsInRole("Admin"))
            {
                customers.Where(c => c.user_id == user_id);
            }
            ViewBag.customers = customers.ToList();
            ViewBag.cities = db.cities.ToList();
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> New([Bind(Include = "task_id, name, last_name, customer_id, address, city, town, phone")] Order order)
        {
            var user_id = User.Identity.GetUserId();

            if (!ModelState.IsValid) return RedirectToAction("New/" + order.task_id, "Order", new { success = "failed" });

            Order o = new Order
            {
                name = order.name,
                last_name = order.last_name,
                phone = order.phone,
                task_id = order.task_id,
                user_id = user_id,
                address = order.address,
                customer_id = order.customer_id,
                city = order.city,
                town = order.town
            };

            db.orders.Add(o);
            db.SaveChanges();

            Tasks task_ = db.tasks.Where(t => t.id == order.task_id).FirstOrDefault();
            if(task_ != null)
            {
                task_.is_ordered = true;
                task_.state = true;
                db.SaveChanges();
            }
            return RedirectToAction("Detail/" + o.id, "Order");

        }
        [Authorize]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            //try
            //{
            //    ApplicationUser s = db.Users.Where(q => q.Id == id).FirstOrDefault();
            //    if (s == null) return new HttpStatusCodeResult(404);
            //    db.Users.Remove(s);
            //    db.SaveChanges();
            //    return new HttpStatusCodeResult(200);
            //}
            //catch (Exception ex)
            //{
            //    return new HttpStatusCodeResult(500);
            //    throw;
            //}
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult Town(int id)
        {
            return Json(db.towns.Where(c => c.city_id == id).ToList());
        }


        //[Authorize]
        //[HttpPost]
        //public async Task<ActionResult> Add([Bind(Include = "password,name,surname,role,username")] PersonnelView person)
        //{
        //    if (!ModelState.IsValid) return RedirectToAction("Index", "Personnel", new { success = "failed" });

        //    UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
        //    UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

        //    ApplicationUser user = new ApplicationUser
        //    {
        //        Name = person.Name,
        //        Surname = person.Surname,
        //        Email = "user@mail.com",
        //        UserName = person.Username
        //    };

        //    var result = await userManager.CreateAsync(user, person.Password);

        //    if (!result.Succeeded) return RedirectToAction("Index", "Personnel", new { success = "failed" });

        //    userManager.AddToRole(user.Id, person.Role);
        //    return RedirectToAction("Index", "Personnel", new { success = "ok" });
        //}

        //[HttpPost]
        //[Authorize]
        //public ActionResult Update(string id, [Bind(Include = "password,name,surname,role,username")] PersonnelView person)
        //{
        //    id = id.Trim();
        //    UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
        //    UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

        //    ApplicationUser user = userManager.FindById(id.Trim());
        //    if(user == null) return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Personnel", action = "Detail", Id = id, success = "failed" }));

        //    user.Name = person.Name;
        //    user.Surname = person.Surname;
        //    user.UserName = person.Username;


        //    string role_id = user.Roles.SingleOrDefault().RoleId;
        //    string role_name = db.Roles.SingleOrDefault(r => r.Id == role_id).Name;
        //    if(role_name.ToLower().Trim() != person.Role.ToLower().Trim())
        //    {
        //        userManager.RemoveFromRole(user.Id, role_name);
        //        userManager.AddToRole(user.Id, person.Role);
        //    }


        //    if (!String.IsNullOrEmpty(person.Password)) user.PasswordHash = userManager.PasswordHasher.HashPassword(person.Password);

        //    var result =  userManager.Update(user);
        //    if (!result.Succeeded) return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Personnel", action = "Detail", Id = id, success = "failed" }));


        //    return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Personnel", action = "Detail", Id = id, success = "ok" }));

        //}
    }
}