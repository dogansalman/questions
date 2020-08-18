using QuestionsSYS.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.ModelViews;
using System.Data.Entity;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using System.Xml.Schema;

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
                                where o.user_id == u.Id && start_date >= o.added && o.added <= end_date
                                select o.id).Count(),
                      order_total = (db.orders.Where(ot => ot.user_id == u.Id && ot.state == "Onaylandı" &&  ot.added >= start_date && ot.added <= end_date).Sum(ff => ff.total)),
                      tasks = (db.question_tasks.Where(t => t.user_id == u.Id && t.added >= start_date && t.added <= end_date).Count()),
                      dates = (db.auth_history.Where(ah => ah.user_id == u.Id && ah.login_date >= start_date && ah.logout_date <= end_date).ToList()),
                      total_work_time = ""
                  }
                );


                var rp = reports.ToList();

                if (reports != null)
                {
                    rp.ForEach(user =>
                    {
                        double total_time = 0;
                        user.dates.ForEach(time => {
                            double tm = (time.logout_date - time.login_date).TotalHours;
                            if(tm >= 0) {
                                total_time += tm;
                            }
                        });
                        TimeSpan tsp =  TimeSpan.FromHours((double)total_time);
                        //new DateTime(tsp.Ticks).ToString("HH:mm:ss");
                        string tstring = new DateTime(tsp.Ticks).ToString("HH") + " Saat " + new DateTime(tsp.Ticks).ToString("mm") + " Dk.";
                        user.total_work_time = tstring;
                    });
                }

                return View(rp);
            } 
            return View();
        }
    }
}