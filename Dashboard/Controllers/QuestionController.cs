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

namespace QuestionsSYS.Controllers
{
    public class QuestionController : Controller
    {

        DatabaseContexts db = new DatabaseContexts();
        // GET: Question
        public ActionResult Index()
        {
            return View(db.questions.ToList().OrderByDescending(q => q.id));
        }

        public ActionResult New([Bind(Include = "question,note,state,name,lastname,phone,phone2,source")] Question model)
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
                        string source_name = ds.Tables[0].Rows[i][6].ToString();
                        Soruce s = db.sources.Where(sc => sc.name == source_name).FirstOrDefault();
                        if(s == null)
                        {
                            Soruce n_sc = new Soruce
                            {
                                name = ds.Tables[0].Rows[i][6].ToString()
                            };
                            db.sources.Add(n_sc);
                            db.SaveChanges();
                        }


                        Question q = new Question
                        {
                            name = ds.Tables[0].Rows[i][0].ToString(),
                            lastname = ds.Tables[0].Rows[i][1].ToString(),
                            phone = ds.Tables[0].Rows[i][2].ToString(),
                            phone2 = ds.Tables[0].Rows[i][3].ToString(),
                            question = ds.Tables[0].Rows[i][4].ToString(),
                            note = ds.Tables[0].Rows[i][5].ToString(),
                            source = ds.Tables[0].Rows[i][6].ToString(),
                            added = DateTime.Now,


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
        public ActionResult Add([Bind(Include = "question,note,state,name,lastname,phone,phone2,source")] Question model)
        {
            if (!ModelState.IsValid) return RedirectToAction("New", "Question", new { success = "failed" });

            Question q = new Question
            {
                added = DateTime.Now,
                lastname = model.lastname,
                name = model.name,
                note = model.note,
                phone = model.phone,
                phone2 = model.phone2,
                source = model.source,
                question = model.question,
                state = model.state
            };

            db.questions.Add(q);
            db.SaveChanges();
            return RedirectToAction("New", "Question", new { success = "ok" });
        }

        [HttpPost]
        public ActionResult Update(int? id, [Bind(Include = "question,note,state,name,lastname,phone,phone2,source")] Question model)
        {
            Question q = db.questions.Where(qu => qu.id == id).FirstOrDefault();

            if (q == null) return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Question", action = "Detail", Id = id, success = "failed" }));


            q.lastname = model.lastname;
            q.name = model.name;
            q.note = model.note;
            q.phone = model.phone;
            q.phone2 = model.phone2;
            q.source = model.source;
            q.question = model.question;
            q.state = model.state;
            q.note = model.note;
          
            db.SaveChanges();
            return  RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Question", action = "Detail", Id = id, success = "ok" }));
        }

      
    }
}