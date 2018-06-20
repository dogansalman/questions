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
        private IdentityContexts db_identity = new IdentityContexts();
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
            string totalQuestion = db.questions.ToList().Count().ToString();
            string totalPersonnel = db_identity.Users.ToList().Count().ToString();
            ViewBag.totalQuestion = totalQuestion;
            ViewBag.totalPersonnel = totalPersonnel;

            return View();
        }

        public ActionResult Dashboard_5()
        {

            return View();
        }

    }
}