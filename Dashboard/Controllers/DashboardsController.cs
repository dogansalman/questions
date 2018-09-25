using QuestionsSYS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Context;
using System.Security.Claims;
using System.Globalization;

namespace QuestionsSYS.Controllers
{
    [Authorize]
    public class DashboardsController : Controller
    {
        private DatabaseContexts db = new DatabaseContexts();
         
        public ActionResult Dashboard_1()
        {
            return View();
        }

        public ActionResult Dashboard_2()
        {
            return View();
        }

        public ActionResult Dashboard_3()
        {
            return View();
        }
        
        public ActionResult Dashboard_4()
        {
            return View();
        }
       
        public ActionResult Index(string date = null)
        {

            if (User.IsInRole("Admin"))
            {
                
                string totalQuestion = db.questions.ToList().Count().ToString();
                string totalPersonnel = db.Users.ToList().Count().ToString();
                string totalTasks = db.tasks.ToList().Count().ToString();
                string ordered_tasks = db.tasks.Where(t => t.state == true).Count().ToString();

                ViewBag.totalQuestion = totalQuestion;
                ViewBag.totalPersonnel = totalPersonnel;
                ViewBag.totalTasks = totalTasks;
                ViewBag.ordertedTasks = ordered_tasks;
                ViewBag.personnel = db.Users.ToList();
              
            }



            return View();
        }


        public ActionResult Reports(string date = null)
        {
            if (!User.IsInRole("Admin")) return new HttpStatusCodeResult(403);

            List<PersonnelReports> personnelList = new List<PersonnelReports>();
            List<string> order_states = db.tasks.Select(t => t.order_state).Distinct().ToList();

            if (date != null)
            {
                DateTime date1_ = DateTime.ParseExact(date.Split('-')[0].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime date2_ = DateTime.ParseExact(date.Split('-')[1].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                foreach (var item in db.Users.ToList())
                {

                    PersonnelReports person = new PersonnelReports
                    {
                        fullname = item.Name + " " + item.Surname,
                        tasks = db.tasks.Where(t => t.user_id == item.Id && t.created_date > date1_ && t.created_date < date2_).Count(),
                        states = new List<PersonnelOrderState>()
                    };

                    order_states.ForEach(stateName =>
                    {
                        var counts = db.tasks.Count(t => t.user_id == item.Id && t.order_state == stateName && t.created_date > date1_ && t.created_date < date2_);
                        PersonnelOrderState pos = new PersonnelOrderState { state = stateName, count = counts };
                        person.states.Add(pos);
                    });

                    personnelList.Add(person);
                }

                IEnumerable<PersonnelReports> model = new List<PersonnelReports>(personnelList);
                return View(model);

            }


            
            foreach (var item in db.Users.ToList())
            {

                PersonnelReports person = new PersonnelReports
                {
                    fullname = item.Name + " " + item.Surname,
                    tasks = db.tasks.Where(t => t.user_id == item.Id).Count(),
                    states = new List<PersonnelOrderState>()
                };

                order_states.ForEach(stateName =>
                {
                    var counts = db.tasks.Count(t => t.user_id == item.Id && t.order_state == stateName);
                    PersonnelOrderState pos = new PersonnelOrderState { state = stateName, count = counts };
                    person.states.Add(pos);
                });

                personnelList.Add(person);
            }

            IEnumerable<PersonnelReports> model_ = new List<PersonnelReports>(personnelList);
            return View(model_);



        }
        public ActionResult Dashboard_5()
        {

            return View();
        }

    }
}