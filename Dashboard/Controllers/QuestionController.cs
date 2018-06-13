using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.Models;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Context;
namespace QuestionsSYS.Controllers
{
    public class QuestionController : Controller
    {

        DatabaseContexts db = new DatabaseContexts();
        // GET: Question
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult New()
        {
            ViewBag.soruces = db.sources.ToList();
            return View();
        }
    }
}