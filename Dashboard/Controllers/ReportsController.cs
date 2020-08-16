using QuestionsSYS.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.ModelViews;
using System.Data.Entity;
using System.Web.UI.WebControls;

namespace QuestionsSYS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        // GET: Reports
       
        DatabaseContexts db = new DatabaseContexts();
        public ActionResult Index(string date = null)
        {
    


            if (date != null)
            {
                string[] dates = date.Split('-');
                DateTime start_date = Convert.ToDateTime(dates[0].Trim());
                DateTime end_date = Convert.ToDateTime(dates[1].Trim());
                var reports = (
                  from u in db.Users
                  select new ReportsView
                  {
                      employee_name = u.Name + " " + u.Surname,
                      orders = (from o in db.orders
                                where o.user_id == u.Id && o.added >= start_date && o.added <= end_date
                                select o.id).Count(),
                      order_total = (db.orders.Where(ot => ot.user_id == u.Id && ot.state == "Onaylandı" && ot.added >= start_date && ot.added <= end_date).Sum(ff => ff.total)),
                      tasks = (db.question_tasks.Where(t => t.user_id == u.Id && t.added >= start_date && t.added <= end_date).Count()),
                      total_work_time = "12"
                  }
                );
                return View(reports);
            } 
            return View();
        }
    }
}