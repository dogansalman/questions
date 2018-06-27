using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.Models;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Context;
using System.Web.Routing;
using System.Data;
using System.Data.OleDb;
using PagedList;
using QuestionsSYS.Identity;

namespace QuestionsSYS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionController : Controller
    {

        DatabaseContexts db = new DatabaseContexts();
        

        // GET: Question
      
        public ActionResult Index(int page = 1)
        {
            ViewBag.personnel = db.Users.ToList();
            return View();
        }
 
        public ActionResult Tasks(int page = 1)
        {
            PagedList<Question> model = new PagedList<Question>(db.questions.Where(qu => !qu.state).OrderByDescending(q => q.added), page, 20);
            return View(model);
        }

        public ActionResult All(int page = 1)
        {

            var questions = (
              from q in db.questions
              select new QuestionsListView
              {
                  added = q.added,
                  id = q.id,
                  fullname = q.fullname,
                  note = q.note,
                  phone = q.phone,
                  phone2 = q.phone2,
                  question = q.question,
                  source = q.source,
                  state = q.state,
                  user_fullname = (from ut in db.question_tasks
                                   where ut.question_id == q.id
                                   join u in db.Users on ut.user_id equals u.Id
                                   select u.Name + " " + u.Surname
                                   ).FirstOrDefault()

               }
               );
             


            PagedList<QuestionsListView> model = new PagedList<QuestionsListView>(questions.OrderByDescending(q => q.added), page, 20);
            return View(model);
        }

        public ActionResult New([Bind(Include = "question,note,state,fullname,phone,phone2,source")] Question model)
        {
            ViewBag.soruces = db.sources.ToList();
            return View();

        }
        public ActionResult Detail(int? id)
        {
            ViewBag.soruces = db.sources.ToList();
            Question q = db.questions.Where(qu => qu.id == id).FirstOrDefault();
            if (q == null) return new HttpStatusCodeResult(404);
            return View(q);
        }
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            try
            {
                Question s = db.questions.Where(q => q.id == id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                db.questions.Remove(s);
                db.SaveChanges();
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Question", action = "Index", Id = id, success = "failed_remove" }));
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500);
                throw;
            }
        }
        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportExcelFileToDatabase(HttpPostedFileBase FileUpload)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Request.Files["FileUpload"].ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["FileUpload"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {

                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["FileUpload"].SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                        excelConnection.Close();
                        excelConnection.Dispose();
                        excelConnection1.Close();
                        excelConnection1.Dispose();

                    }


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string source_name = ds.Tables[0].Rows[i][5].ToString();
                        Soruce s = db.sources.Where(sc => sc.name == source_name).FirstOrDefault();
                        if(s == null)
                        {
                            Soruce n_sc = new Soruce
                            {
                                name = ds.Tables[0].Rows[i][5].ToString()
                            };
                            db.sources.Add(n_sc);
                            db.SaveChanges();
                        }

                        var date =  String.IsNullOrEmpty(ds.Tables[0].Rows[i][6].ToString()) ? DateTime.Now :  Convert.ToDateTime(ds.Tables[0].Rows[i][6].ToString());

                        Question q = new Question
                        {
                            fullname = ds.Tables[0].Rows[i][0].ToString(),
                            phone = ds.Tables[0].Rows[i][1].ToString(),
                            phone2 = ds.Tables[0].Rows[i][2].ToString(),
                            question = ds.Tables[0].Rows[i][3].ToString(),
                            note = ds.Tables[0].Rows[i][4].ToString(),
                            source = ds.Tables[0].Rows[i][5].ToString(),
                            added = date


                        };

                        db.questions.Add(q);
                        db.SaveChanges();

                    }

                   return  RedirectToAction("Import", new RouteValueDictionary(new { controller = "Question", action = "Import",  success = "ok" }));
                }
                else
                {
                   return  RedirectToAction("Import", new RouteValueDictionary(new { controller = "Question", action = "Import", success = "failed"}));
                }

              

            }
            catch (Exception ex)
            {
                return RedirectToAction("Import", new RouteValueDictionary(new { controller = "Question", action = "Import", success = "failed", msg = ex.Message.ToString() }));
            }
        }

        [HttpPost]
        public ActionResult Add([Bind(Include = "question,note,state,fullname,phone,phone2,source")] Question model)
        {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);

            
            if (!String.IsNullOrEmpty(model.phone))
            {
                if (db.questions.Any(qu => qu.phone == model.phone || qu.phone2 == model.phone ))
                {
                    return new HttpStatusCodeResult(501);
                }
            }

            if (!String.IsNullOrEmpty(model.phone2))
            {
                if (db.questions.Any(qu => qu.phone == model.phone2 || qu.phone2 == model.phone2))
                {
                    return new HttpStatusCodeResult(501);
                }
            }



            Question q = new Question
            {
                added = DateTime.Now,
                fullname = model.fullname,
                note = model.note,
                phone = model.phone,
                phone2 = model.phone2,
                source = model.source,
                question = model.question,
                state = model.state
            };

            db.questions.Add(q);
            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult Update(int? id, [Bind(Include = "question,note,state,fullname,phone,phone2,source")] Question model)
        {
            Question q = db.questions.Where(qu => qu.id == id).FirstOrDefault();

            if (q == null) return new HttpStatusCodeResult(404);

            q.fullname = model.fullname;
            q.note = model.note;
            q.phone = model.phone;
            q.phone2 = model.phone2;
            q.source = model.source;
            q.question = model.question;
            q.state = model.state;
            q.note = model.note;
          
            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

      
    }
}