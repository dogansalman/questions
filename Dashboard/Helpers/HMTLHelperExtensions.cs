using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using QuestionsSYS.Env;
using QuestionsSYS.Context;

namespace QuestionsSYS
{
    public static class HMTLHelperExtensions
    {
      public static IdentityContexts db = new IdentityContexts();

        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass)) 
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        public static string SetImage(this HtmlHelper html, string url, int width, int height)
        {
            if (String.IsNullOrEmpty(url)) return "/Images/no-photo.png";
            return width > 0 && height > 0 ? Env.Env.cloudinaryUrl + "/" + "w_" + width + "," + "h_" + height + ",c_fill/" + url : Env.Env.cloudinaryUrl + "/" + url;    
        }

        public static string IdentityFullName(this HtmlHelper html)
        {
            var userIdentity = html.ViewContext.HttpContext.User.Identity;   
            if (userIdentity == null) return "";

            var userDetail = db.Users.SingleOrDefault(u => u.UserName == userIdentity.Name);
            if (userDetail != null) return userDetail.Name + " " + userDetail.Surname;
            return "";
        }



    }
}
