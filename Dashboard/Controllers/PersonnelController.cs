﻿using System;
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

        DatabaseContexts db = new DatabaseContexts();
        [Authorize(Roles = "Admin")]
        // GET: Personnel
        public ActionResult Index()
        {

            var Personnels = (from user in db.Users

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
                             );
            return View(Personnels);
        }
        [Authorize]
        public ActionResult Detail(string id)
        {
            ViewBag.employee_type = db.employee_type.ToList();
            var Person = (from user in db.Users
                          where user.Id == id
                          select new PersonnelView
                          {
                              Id = user.Id,
                              Name = user.Name,
                              Surname = user.Surname,
                              Username = user.UserName,
                              color = user.color,
                              employee_type = user.employee_type,
                              Role = (from userRole in user.Roles
                                      where userRole.UserId == user.Id
                                      join role in db.Roles on userRole.RoleId
                                      equals role.Id
                                      select role.Name).FirstOrDefault()
                          }
                 ).FirstOrDefault();
            
            if (Person == null) return RedirectToAction("Index", "Personnel");

            if (Person.color == null) Person.color = "#1ab394";
            return View(Person);
            

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                ApplicationUser s = db.Users.Where(q => q.Id == id).FirstOrDefault();
                if (s == null) return new HttpStatusCodeResult(404);
                db.Users.Remove(s);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500);
                throw;
            }
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize]
        public ActionResult Update(string id, [Bind(Include = "password,name,surname,role,username, color, employee_type")] PersonnelView person)
        {
            id = id.Trim();
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            
            ApplicationUser user = userManager.FindById(id.Trim());
            if(user == null) return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Personnel", action = "Detail", Id = id, success = "failed" }));

            user.Name = person.Name;
            user.Surname = person.Surname;
            user.UserName = person.Username;
            user.color = person.color;
            user.employee_type = person.employee_type;

            string role_id = user.Roles.SingleOrDefault().RoleId;
            string role_name = db.Roles.SingleOrDefault(r => r.Id == role_id).Name;
            if(role_name.ToLower().Trim() != person.Role.ToLower().Trim())
            {
                userManager.RemoveFromRole(user.Id, role_name);
                userManager.AddToRole(user.Id, person.Role);
            }
           

            if (!String.IsNullOrEmpty(person.Password)) user.PasswordHash = userManager.PasswordHasher.HashPassword(person.Password);

            var result =  userManager.Update(user);
            if (!result.Succeeded) return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Personnel", action = "Detail", Id = id, success = "failed" }));


            return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Personnel", action = "Detail", Id = id, success = "ok" }));

        }
    }
}