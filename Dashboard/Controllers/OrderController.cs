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
using System.EnterpriseServices;
using Microsoft.Ajax.Utilities;
using System.ComponentModel.DataAnnotations;

namespace QuestionsSYS.Controllers
{
    public class Discount
    {
        [Required]
        public float discount { get; set; }
    }
    public class State
    {
        [Required]
        public string state { get; set; }
    }

    public class OrderController : Controller
    {
        public void calculateOrderTotal(int id)
        {
            float order_total = 0;
            Order o = db.orders.Where(order => order.id == id).FirstOrDefault();


            var opList = db.order_products.Where(op => op.orderId == id).ToList();
            if (opList != null)
            {
                opList.ForEach(op =>
                {
                    order_total += op.qty * op.price;
                });
                o.total = order_total;
                db.SaveChanges();
            }
        }

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
            
            if (User.IsInRole("Admin") || User.IsInRole("Logistics User"))
            {
                return View(orders.ToList());
            } 
            else
            {
                return View(orders.Where(o => o.user_id == user_id).ToList());
            }
           

            
        }


        [Authorize]
        [HttpPost]
        public ActionResult ProductUpdate(int order_id, [Bind(Include = "id, product_name, qty, price")] QuestionsSYS.ModelViews.Product model)
        {
            var user_id = User.Identity.GetUserId();
            var order = db.orders;
            order.Where(o => o.id == order_id);
            if (!User.IsInRole("Admin"))
            {
                order.Where(o => o.user_id == user_id && o.id == order_id);
            };
            var order_detail = order.FirstOrDefault();

            if (order_detail != null && order_detail.state == "Onay Bekliyor")
            {
                var is_exist_product = db.order_products.Where(o => o.orderId == order_id && o.product_name == model.product_name).ToList();
                if (is_exist_product != null && is_exist_product.Count > 1)
                {
                    return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = order_id, exist_product = "1" }));
                }

                OrderProduct op_ = db.order_products.Where(opp => opp.id == model.id).FirstOrDefault();
                if (op_ != null)
                {
                    op_.price = model.price;
                    op_.qty = model.qty;
                    op_.product_name = model.product_name;
                    db.SaveChanges();

                    calculateOrderTotal(order_id);
                }

                return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = order_id, success = "ok" }));

            }
            return new HttpStatusCodeResult(500);


        }


        [Authorize]
        [HttpPost]
        public ActionResult Discount(int order_id, [Bind(Include = "discount, order_id")] Discount model)
        {
            var user_id = User.Identity.GetUserId();
            Order order;

            if (!User.IsInRole("Admin")) {
                order = db.orders.Where(o => o.user_id == user_id && o.id == order_id).FirstOrDefault();
            } else
            {
                order = db.orders.Where(o => o.id == order_id).FirstOrDefault();
            }

            if(model.discount >= order.total)
            {
                return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = order_id, success = "failed" }));
            }
            if (order != null && order.state == "Onay Bekliyor")
            {
                order.discount = model.discount;
            }
            db.SaveChanges();

            return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = order_id, success = "ok" }));
        }

        [Authorize]
        [HttpPost]
        public ActionResult State(int order_id, [Bind(Include = "state")] State model)
        {
            var user_id = User.Identity.GetUserId();
            Order order;

            if (User.IsInRole("Admin") || User.IsInRole("Logistics User"))
            {
                order = db.orders.Where(o => o.id == order_id).FirstOrDefault();
                if(order != null)
                {
                    order.state = model.state;
                    db.SaveChanges();
                    return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = order_id, success = "ok" }));
                }
            }
            return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = order_id, success = "failed" }));
        }

        [Authorize]
        [HttpPost]
        public ActionResult ProductsAdd(int id, [Bind(Include = "id, product_name, qty, price")] QuestionsSYS.ModelViews.Product model)
        {
            var user_id = User.Identity.GetUserId();
            Order order;
         
            if(!User.IsInRole("Admin"))
            {
                order = db.orders.Where(o => o.user_id == user_id && o.id == id).FirstOrDefault();
            } else
            {
                order = db.orders.Where(o => o.id == id).FirstOrDefault();
            }

            
            if (order != null && order.state == "Onay Bekliyor")
            {
                var is_exist_product = db.order_products.Where(o => o.orderId == id && o.product_name == model.product_name).FirstOrDefault();
                if(is_exist_product != null)
                {
                    return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = id, exist_product = "1" }));
                }
                OrderProduct op = new OrderProduct
                {
                    price = model.price,
                    product_name = model.product_name,
                    qty = model.qty,
                    orderId = id
                };
                db.order_products.Add(op);
                db.SaveChanges();
                calculateOrderTotal(id);
                return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Order", action = "Detail", id = id, success = "ok" }));

     
            }
            return new HttpStatusCodeResult(500);

        }
        [Authorize]
        public ActionResult Detail(int id)
        {
            ViewBag.cargo = db.cargo.ToList();
            ViewBag.products = db.products.ToList();

            var user_id = User.Identity.GetUserId();
            ViewBag.task_id = id;
            var customers = (
               from t in db.customers
               join c in db.cities on t.city equals c.id
               join d in db.towns on t.town equals d.id
               select new CustomerList
               {
                   id = t.id,
                   name = t.name,
                   lastname = t.lastname,
                   city = c.city_name,
                   town = d.town_name
               }
               ).ToList();
            
            if (!User.IsInRole("Admin") || !User.IsInRole("Logistics User"))
            {
                customers.Where(c => c.user_id == user_id);
            }
            ViewBag.customers = customers.ToList();
            ViewBag.cities = db.cities.ToList();
            


            var order_ = db.orders.Where(s => s.id == id);
            if (!User.IsInRole("Admin") || !User.IsInRole("Logistics User"))
            {
                order_.Where(c => c.user_id == user_id);
            }
            Order o = order_.FirstOrDefault();

            o.order_products = db.order_products.Where(op => op.orderId == o.id).ToList();

            ViewBag.towns = db.towns.Where(t => t.city_id == o.city).ToList();

            return View(o);
        }

        [Authorize]
        [HttpPost]

        public ActionResult DeleteProduct(int? id)
        {
            
            var op = db.order_products.Where(_op => _op.id == id).FirstOrDefault();
            Order o = db.orders.Where(_o => _o.id == op.orderId).FirstOrDefault();
            var user_id = User.Identity.GetUserId();
         
            if (!User.IsInRole("Admin"))
            {
                if(o.user_id != user_id && o.id == id)
                {
                    return RedirectToAction("Detail/" + o.id, "Order", new { success = "failed" });

                }
            }
            if (o != null && o.state == "Onay Bekliyor")
            {
                db.order_products.Remove(op);
                db.SaveChanges();
                calculateOrderTotal(op.orderId);
                return RedirectToAction("Detail/" + o.id, "Order");
            }
            
            return RedirectToAction("Detail/" + o.id, "Order", new { success = "failed" });

        }

        [Authorize]
        [HttpPost]
        public ActionResult Update(int id, [Bind(Include = "task_id, name, last_name, customer_id, address, city, town, phone, cargo, cargo_barcode")] Order model)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Detail/" + id, "Order", new { success = "failed" });
            }
            ViewBag.products = db.products.ToList();

            var user_id = User.Identity.GetUserId();
            var order = db.orders.Where(o => o.id == id);
            if(!User.IsInRole("Admin"))
            {
                order.Where(o => o.user_id == user_id);
            }
            Order order_detail = order.FirstOrDefault();
            if(order_detail != null && order_detail.state == "Onay Bekliyor")
            {
                order_detail.name = model.name;
                order_detail.last_name = model.last_name;
                order_detail.customer_id = model.customer_id;
                order_detail.address = model.address;
                order_detail.city = model.city;
                order_detail.town = model.town;
                order_detail.phone = model.phone;

                if(User.IsInRole("Logistics User") || User.IsInRole("Admin"))
                {
                    order_detail.cargo = model.cargo;
                    order_detail.cargo_barcode = model.cargo_barcode;
                }
       
                db.SaveChanges();
                return RedirectToAction("Detail/" + id, "Order", new { success = "ok" });
            }
            
            return RedirectToAction("Detail/" + id, "Order", new { success = "failed" });
        }

        [Authorize]
        public ActionResult New(int? id)
        {
            
            if(id != null && id != 0)
            {
                Tasks ta = db.tasks.Where(i => i.id == id).FirstOrDefault();
                
                Question q = db.questions.Where(s => s.id == ta.question_id).FirstOrDefault();

                if (ta == null || q == null)
                {
                    return new HttpStatusCodeResult(404);
                }
                ViewBag.task_phone = q.phone;

                string[] ssizes = q.fullname.Split(' ', '\t');

                for (int i = 0; i < ssizes.Length; i++)
                {
                    if (i != ssizes.Length - 1)
                    {
                        ViewBag.name += " " + ssizes[i];
                    }
                }
                ViewBag.lastname = ssizes[ssizes.Length - 1];
            }
            

            var user_id = User.Identity.GetUserId();
            ViewBag.task_id = id;
            var customers = (
               from t in db.customers
               join c in db.cities on t.city equals c.id
               join d in db.towns on t.town equals d.id
               select new CustomerList
               {
                   id = t.id,
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
        public async Task<ActionResult> New([Bind(Include = "customer_id, task_id, name, last_name, address, city, town, phone, cargo, cargo_barcode")] Order order)
        {
            var user_id = User.Identity.GetUserId();

            if (order.customer_id == 0)
            {
                Customer c = new Customer
                {
                    name = order.name,
                    lastname = order.last_name,
                    address = order.address,
                    town = order.town,
                    city = order.city,
                    job = "Belirtilmedi",
                    birth_date = DateTime.Now,
                    phone = order.phone,
                    user_id = user_id
                };
                db.customers.Add(c);
                db.SaveChanges();
                order.customer_id = c.id;
            }

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
                town = order.town,
                cargo = order.cargo,
                cargo_barcode = order.cargo_barcode
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
        public ActionResult Delete(int id)
        {
            var user_id = User.Identity.GetUserId();
            Order order = db.orders.Where(o => o.id == id).FirstOrDefault();
            if(order.state != "Onay Bekliyor") return new HttpStatusCodeResult(403);

            if (order == null) return new HttpStatusCodeResult(404);

            if(order.user_id != user_id)
            {
                if ((!User.IsInRole("Admin") && !User.IsInRole("Logistics User")))
                {
                    return new HttpStatusCodeResult(403);
                }
            }
            db.orders.Remove(order);
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult Town(int id)
        {
            return Json(db.towns.Where(c => c.city_id == id).ToList());
        }

    }
}