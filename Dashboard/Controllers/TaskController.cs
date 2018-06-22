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
    public class TaskController : Controller
    {
        DatabaseContexts db = new DatabaseContexts();
        IdentityContexts db_identity = new IdentityContexts();

        // GET: Task
        public ActionResult Index()
        {
            /*
                     var user_id = User.Identity.GetUserId();
            var user = db_identity.Users.Where(u => u.Id == user_id).FirstOrDefault();
            var roleName =  (from userRole in user.Roles
             where userRole.UserId == user.Id
             join role in db_identity.Roles on userRole.RoleId
             equals role.Id
             select role.Name).FirstOrDefault();
             */




            return View();
        }



        public ActionResult All(int page = 1, string SearchString = null)
        {
            ViewBag.SearchString = SearchString;

            var user_id = User.Identity.GetUserId();
            var tasks = (
               from t in db.tasks
               join q in db.questions on t.question_id equals q.id
               where t.user_id == user_id
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
                   state = t.state
               }
               );

            if(SearchString != null)
            {
                tasks = tasks.Where(t => t.phone.Contains(SearchString) || t.phone2.Contains(SearchString) || t.fullname.Contains(SearchString));
            }

            PagedList<TaskListView> model = new PagedList<TaskListView>(tasks.OrderByDescending(ta => ta.state), page, 20);
            return View(model);
            
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
                    db.tasks.Add(new Tasks { created_date = DateTime.Now, question_id = Convert.ToInt32(q), user_id = model.user_id });
                    db.SaveChanges();
                });
               
            }
            catch (Exception exs)
            {
                return new HttpStatusCodeResult(500, exs.Message.ToString());
            }
          
            return new HttpStatusCodeResult(200);
        }
    }
}