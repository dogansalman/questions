using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QuestionsSYS.Context;
using QuestionsSYS.ModelViews;
using QuestionsSYS.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web.Routing;

namespace QuestionsSYS.Controllers
{
    public class PersonnelController : Controller
    {

        IdentityContexts db = new IdentityContexts();
     
        // GET: Personnel
        public ActionResult Index()
        {

            var Personnels = (from user in db.Users

                              select new PersonnelView
                              {
                                  Id = user.Id,
                                  Name = user.Name,
                                  Surname = user.Surname,
                                  Role = (from userRole in user.Roles
                                          where userRole.UserId == user.Id
                                          join role in db.Roles on userRole.RoleId
                                          equals role.Id
                                          select role.Name).FirstOrDefault()
                              }
                             );
            return View(Personnels);
        }

        public ActionResult Detail(string id)
        {
            var Person = (from user in db.Users
                          where user.Id == id
                          select new PersonnelView
                          {
                              Id = user.Id,
                              Name = user.Name,
                              Surname = user.Surname,
                              Username = user.UserName,
                              Role = (from userRole in user.Roles
                                      where userRole.UserId == user.Id
                                      join role in db.Roles on userRole.RoleId
                                      equals role.Id
                                      select role.Name).FirstOrDefault()
                          }
                 ).FirstOrDefault();

            if (Person == null) return RedirectToAction("Index", "Personnel");

            return View(Person);
            

        }
        

        [HttpPost]
        public async Task<ActionResult> Add([Bind(Include = "password,name,surname,role,username")] PersonnelView person)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Personnel", new { success = "failed" });

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            ApplicationUser user = new ApplicationUser
            {
                Name = person.Name,
                Surname = person.Surname,
                Email = "user@mail.com",
                UserName = person.Username
            };

            var result = await userManager.CreateAsync(user, person.Password);

            if (!result.Succeeded) return RedirectToAction("Index", "Personnel", new { success = "failed" });

            userManager.AddToRole(user.Id, person.Role);
            return RedirectToAction("Index", "Personnel", new { success = "ok" });
        }
        [HttpPost]
        public ActionResult Update(string id, [Bind(Include = "password,name,surname,role,username")] PersonnelView person)
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            
            ApplicationUser user = userManager.FindById(id);
            user.Name = person.Name;
            user.Surname = person.Surname;
            user.UserName = person.Username;
            
            if (!String.IsNullOrEmpty(person.Password)) user.PasswordHash = userManager.PasswordHasher.HashPassword(person.Password);

            var result =  userManager.Update(user);
            if (!result.Succeeded) return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Personnel", action = "Detail", Id = id, success = "failed" }));


            return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Personnel", action = "Detail", Id = id, success = "ok" }));

        }
    }
}