using QuestionsSYS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Context;

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
            return View();
        }

        public ActionResult Dashboard_5()
        {
            return View();
        }

    }
}