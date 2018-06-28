using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Context;
using QuestionsSYS.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace QuestionsSYS.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        DatabaseContexts db = new DatabaseContexts();
       
        // GET: Task
        
        public ActionResult Index()
        {
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

        public ActionResult All(int page = 1, string SearchString = null, string type = null)
        {
            ViewBag.SearchString = SearchString;

            var user_id = User.Identity.GetUserId();

            var tasks = (
               from t in db.tasks
               join q in db.questions on t.question_id equals q.id
               select new TaskListView
               {
                   id = t.id,
                   created_date = q.added,
                   fullname = q.fullname,
                   note = q.note,
                   order_state = t.order_state,
                   phone = q.phone,
                   phone2 = q.phone2,
                   question = q.question,
                   source = q.source,
                   state = t.state,
                   user_id = t.user_id,
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


            PagedList<TaskListView> model = new PagedList<TaskListView>(tasks.OrderByDescending(ta => ta.created_date), page, 20);
            return View(model);
            
        }

        public ActionResult uncomplate(int page = 1, string SearchString = null, string type = null)
        {
            ViewBag.SearchString = SearchString;

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
                   note = q.note,
                   order_state = t.order_state,
                   phone = q.phone,
                   phone2 = q.phone2,
                   question = q.question,
                   source = q.source,
                   state = t.state,
                   user_id = t.user_id,
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

            PagedList<TaskListView> model = new PagedList<TaskListView>(tasks.OrderByDescending(ta => ta.created_date), page, 20);
            return View("All", model);
        }

        public ActionResult complate(int page = 1, string SearchString = null, string type = null)
        {
            ViewBag.SearchString = SearchString;

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
                   note = q.note,
                   order_state = t.order_state,
                   phone = q.phone,
                   phone2 = q.phone2,
                   question = q.question,
                   source = q.source,
                   state = t.state,
                   user_id = t.user_id,
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

            PagedList<TaskListView> model = new PagedList<TaskListView>(tasks.OrderByDescending(ta => ta.created_date), page, 20);
            return View("All", model);
        }

        [HttpPost]
        public ActionResult Add(TaskAdd model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);

            try
            {
                using (var db = new DatabaseContexts())
                {
                    var questions = db.questions.Where(q => model.questions.Contains(q.id.ToString())).ToList();
                    questions.ForEach(qs => qs.state = true);
                    db.SaveChanges();
                }

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
            
            try
            {
                Tasks task = db.tasks.Where(t => t.user_id == user_id && t.id == id).FirstOrDefault();
                if(task == null) return new HttpStatusCodeResult(404);


                task.note = model.note;
                task.offer_price = model.offer_price;
                task.order_price = model.order_price;
                task.state = model.state;
                task.order_state = model.order_state;
                task.order_unit = model.order_unit;
                task.feedback_date = model.feedback_date;

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