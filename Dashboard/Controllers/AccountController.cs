using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;
using QuestionsSYS.Identity;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Context;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace QuestionsSYS.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;
        public AccountController()
        {
            DatabaseContexts db = new DatabaseContexts();

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            userManager = new UserManager<ApplicationUser>(userStore);

            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(db);
            roleManager = new RoleManager<ApplicationRole>(roleStore);
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = userManager.Find(model.Username, model.Password);
                if (user != null)
                {
                    IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                    ClaimsIdentity identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    AuthenticationProperties authProps = new AuthenticationProperties();
                    authProps.IsPersistent = true;
                    authManager.SignIn(authProps, identity);

                    
                    AuthService.LoginStateSave(user.Id);

                    return RedirectToAction("Index", "Dashboards");
                }
                else
                {
                    ModelState.AddModelError("LoginUser", "Böyle bir kullanıcı bulunamadı");
                }

            }
          
            return RedirectToAction("Login", "Pages", new
            {
                login = "Failed"
            });
        }

        [Authorize]
        public ActionResult Logout()
        {

            AuthService.LogoutStateSave(User.Identity.GetUserId());
            IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Login", "Pages");
        }
    }
}