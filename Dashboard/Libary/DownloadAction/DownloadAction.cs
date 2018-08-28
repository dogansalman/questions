using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuestionsSYS.Libary.DownloadAction
{
    public class DownloadAction: ActionResult
    {

        public GridView ExcelGridView { get; set; }
        public string fileName { get; set; }


        public DownloadAction(GridView gv, string pFileName)
        {
            ExcelGridView = gv;
            fileName = pFileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {

            //Create a response stream to create and write the Excel file
            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            curContext.Response.Charset = Encoding.Default.ToString();
            curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            curContext.Response.ContentType = "application/vnd.ms-excel";

            //Convert the rendering of the gridview to a string representation 
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            ExcelGridView.RenderControl(htw);

            
            //Open a memory stream that you can use to write back to the response
            byte[] byteArray = Encoding.Default.GetBytes(sw.ToString());
            MemoryStream s = new MemoryStream(byteArray);
            StreamReader sr = new StreamReader(s, Encoding.Default);

            //Write the stream back to the response
            curContext.Response.Write(sr.ReadToEnd());
            curContext.Response.End();

        }

    }
}