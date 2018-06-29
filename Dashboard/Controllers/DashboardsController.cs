using QuestionsSYS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Context;
using System.Security.Claims;

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
       
        public ActionResult Index()
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

            }



            return View();
        }

        public ActionResult Dashboard_5()
        {

            return View();
        }

    }
}