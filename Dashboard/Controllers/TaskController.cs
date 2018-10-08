using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Context;
using QuestionsSYS.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System.Globalization;

namespace QuestionsSYS.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        DatabaseContexts db = new DatabaseContexts();
       
        // GET: Task
        
        public ActionResult Index()
        {
            ViewBag.personnel = db.Users.ToList();
            ViewBag.states = db.states.ToList();
            return View();
        }

        public ActionResult Detail(int? id)
        {
            ViewBag.soruces = db.sources.ToList();
            ViewBag.states = db.states.ToList();
            if (id == null) RedirectToAction("Index", "Task");
            var user_id = User.Identity.GetUserId();

            TaskListView tasks = (
               from t in db.tasks
               join q in db.questions on t.question_id equals q.id
               where t.id == id
               select new TaskListView
               {
                   id = t.id,
                   created_date = q.added,
                   fullname = q.fullname,
                   note = t.note,
                   question_note = q.note,
                   order_state = t.order_state,
                   phone = q.phone,
                   phone2 = q.phone2,
                   question = q.question,
                   source = q.source,
                   state = t.state,
                   order_price = t.order_price,
                   offer_price = t.offer_price,
                   feedback_date = t.feedback_date,
                   contact_date = t.contact_date,
                   order_unit = t.order_unit,
                   user_id = t.user_id,
                   user_fullname = (from u in db.Users
                                    where u.Id == t.user_id
                                    select u.Name + " " + u.Surname
                                    ).FirstOrDefault()
               }
               ).FirstOrDefault();

            if (!User.IsInRole("Admin") && tasks.user_id != user_id) return new HttpStatusCodeResult(404);

            if (tasks == null) return new HttpStatusCodeResult(404);
            return View(tasks);
        }

        public ActionResult All(int page = 1, string SearchString = null, string type = null, string personnel = null)
        {
            ViewBag.SearchString = SearchString;
            ViewBag.personnel = personnel;

            var user_id = User.Identity.GetUserId();

            var tasks = (
               from t in db.tasks
               join q in db.questions on t.question_id equals q.id
               select new TaskListView
               {
                   id = t.id,
                   created_date = q.added,
                   fullname = q.fullname,
                   note = t.note,
                   order_state = t.order_state,
                   phone = q.phone,
                   phone2 = q.phone2,
                   question_note = q.note,
                   question = q.question,
                   source = q.source,
                   state = t.state,
                   user_id = t.user_id,
                   contact_date = t.contact_date,
                   user_fullname = (from u in db.Users
                                    where u.Id == t.user_id
                                    select u.Name + " " + u.Surname 
                                    ).FirstOrDefault()
               }
               );
            if (!User.IsInRole("Admin"))
            {
                tasks = tasks.Where(t => t.user_id == user_id);
            }
            if(SearchString != null)
            {
                tasks = tasks.Where(t => t.phone.Contains(SearchString) || t.phone2.Contains(SearchString) || t.fullname.Contains(SearchString));
            }
            if (type != null)
            {
                tasks = tasks.Where(t => t.order_state.ToLower() == type.ToLower());
            }
            if (User.IsInRole("Admin") && !String.IsNullOrEmpty(personnel))
            {
                tasks = tasks.Where(t => t.user_id == personnel);
            }

            PagedList<TaskListView> model = new PagedList<TaskListView>(tasks.OrderByDescending(ta => ta.created_date), page, 20);
            return View(model);
            
        }

        public ActionResult uncomplate(int page = 1, string SearchString = null, string type = null, string personnel = null)
        {
            ViewBag.SearchString = SearchString;
            ViewBag.personnel = personnel;

            var user_id = User.Identity.GetUserId();
            var tasks = (
               from t in db.tasks
               join q in db.questions on t.question_id equals q.id
               where !t.state
               select new TaskListView
               {
                   id = t.id,
                   created_date = q.added,
                   fullname = q.fullname,
                   note = t.note,
                   question_note = q.note,
                   order_state = t.order_state,
                   phone = q.phone,
                   phone2 = q.phone2,
                   question = q.question,
                   source = q.source,
                   state = t.state,
                   user_id = t.user_id,
                   contact_date = t.contact_date,
                   user_fullname = (from u in db.Users
                                    where u.Id == t.user_id
                                    select u.Name + " " + u.Surname
                                    ).FirstOrDefault()
               }
               );

            if (!User.IsInRole("Admin"))
            {
                tasks = tasks.Where(t => t.user_id == user_id);
            }

            if (SearchString != null)
            {
                tasks = tasks.Where(t => t.phone.Contains(SearchString) || t.phone2.Contains(SearchString) || t.fullname.Contains(SearchString));
            }

            if (type != null)
            {
                tasks = tasks.Where(t => t.order_state.ToLower() == type.ToLower());
            }

            if (User.IsInRole("Admin") && !String.IsNullOrEmpty(personnel))
            {
                tasks = tasks.Where(t => t.user_id == personnel);
            }

            PagedList<TaskListView> model = new PagedList<TaskListView>(tasks.OrderByDescending(ta => ta.created_date), page, 20);
            return View("All", model);
        }

        public ActionResult complate(int page = 1, string SearchString = null, string type = null, string personnel = null)
        {
            ViewBag.SearchString = SearchString;
            ViewBag.personnel = personnel;

            var user_id = User.Identity.GetUserId();
            var tasks = (
               from t in db.tasks
               join q in db.questions on t.question_id equals q.id
               where t.state
               select new TaskListView
               {
                   id = t.id,
                   created_date = q.added,
                   fullname = q.fullname,
                   note = t.note,
                   question_note = q.note,
                   order_state = t.order_state,
                   phone = q.phone,
                   phone2 = q.phone2,
                   question = q.question,
                   source = q.source,
                   state = t.state,
                   user_id = t.user_id,
                   contact_date = t.contact_date,
                   user_fullname = (from u in db.Users
                                    where u.Id == t.user_id
                                    select u.Name + " " + u.Surname
                                    ).FirstOrDefault()
               }
               );

            if (!User.IsInRole("Admin"))
            {
                tasks = tasks.Where(t => t.user_id == user_id);
            }

            if (SearchString != null)
            {
                tasks = tasks.Where(t => t.phone.Contains(SearchString) || t.phone2.Contains(SearchString) || t.fullname.Contains(SearchString));
            }
            if (type != null)
            {
                tasks = tasks.Where(t => t.order_state.ToLower() == type.ToLower());
            }

            if (User.IsInRole("Admin") && !String.IsNullOrEmpty(personnel))
            {
                tasks = tasks.Where(t => t.user_id == personnel);
            }

            PagedList<TaskListView> model = new PagedList<TaskListView>(tasks.OrderByDescending(ta => ta.created_date), page, 20);
            return View("All", model);
        }

        public ActionResult getfeedback(int page = 1, string SearchString = null, string date = null)
        {
            ViewBag.SearchString = SearchString;

            var user_id = User.Identity.GetUserId();

            var tasks = (
               from t in db.tasks
               join q in db.questions on t.question_id equals q.id
               where t.feedback_date != null
               select new TaskListView
               {
                   id = t.id,
                   created_date = q.added,
                   fullname = q.fullname,
                   note = t.note,
                   order_state = t.order_state,
                   phone = q.phone,
                   phone2 = q.phone2,
                   question_note = q.note,
                   question = q.question,
                   source = q.source,
                   state = t.state,
                   user_id = t.user_id,
                   contact_date = t.contact_date,
                   feedback_date = t.feedback_date,
                   user_fullname = (from u in db.Users
                                    where u.Id == t.user_id
                                    select u.Name + " " + u.Surname
                                    ).FirstOrDefault()
               }
               );
            if (!User.IsInRole("Admin"))
            {
                tasks = tasks.Where(t => t.user_id == user_id);
            }
            if (SearchString != null)
            {
                tasks = tasks.Where(t => t.phone.Contains(SearchString) || t.phone2.Contains(SearchString) || t.fullname.Contains(SearchString));
            }
            if (date != null)
            {
               
                var a = date.Split(',')[0];
                var b = date.Split(',')[1];
                DateTime s_date = DateTime.ParseExact(a, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                DateTime e_date = DateTime.ParseExact(b, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                tasks = tasks.Where(t => t.feedback_date >= s_date && t.feedback_date <= e_date);
            }


            PagedList<TaskListView> model = new PagedList<TaskListView>(tasks.OrderByDescending(ta => ta.created_date), page, 20);
            return View("All",model);

        }

        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(TaskAdd model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);

            try
            {

                if (db.question_tasks.Any(qt => model.questions.Contains(qt.question_id.ToString()))) return new HttpStatusCodeResult(501);

                var questions = db.questions.Where(q => model.questions.Contains(q.id.ToString())).ToList();
                
                questions.ForEach(qs => qs.state = true);
                db.SaveChanges();

                model.questions.ToList().ForEach(q =>
                {
                    db.tasks.Add(new Tasks { created_date = DateTime.Now, question_id = Convert.ToInt32(q), user_id = model.user_id, order_state= "İşlem Bekleniyor" });
                    db.question_tasks.Add(new QuestionTasks { question_id = Convert.ToInt32(q), user_id = model.user_id });
                    db.SaveChanges();
                });
               
            }
            catch (Exception exs)
            {
                return new HttpStatusCodeResult(500, exs.Message.ToString());
            }
          
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult Update(int id, TaskListView model)
        {
            
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);
            var user_id = User.Identity.GetUserId();
            Tasks task = new Tasks();

            try
            {
                if (User.IsInRole("Admin"))
                {
                    task = db.tasks.Where(t => t.id == id).FirstOrDefault();
                }
                else
                {
                    task = db.tasks.Where(t => t.user_id == user_id && t.id == id).FirstOrDefault();
                }
             
                if(task == null) return new HttpStatusCodeResult(404);


                task.note = model.note;
                task.offer_price = model.offer_price;
                task.order_price = model.order_price;
                task.state = model.state;
                task.order_state = model.order_state;
                task.order_unit = model.order_unit;
                task.feedback_date = model.feedback_date;
                task.contact_date = model.contact_date;

                db.SaveChanges();
                return new HttpStatusCodeResult(200);

            }
            catch (Exception exs)
            {
                return new HttpStatusCodeResult(500, exs.Message.ToString());
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            try
            {
                Tasks s = db.tasks.Where(q => q.id == id).FirstOrDefault();
                db.question_tasks.RemoveRange(db.question_tasks.Where(x => x.question_id== s.question_id && x.user_id == s.user_id));
                if (s == null) return new HttpStatusCodeResult(404);

                db.tasks.Remove(s);
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