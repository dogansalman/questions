using System;
using System.Linq;
using QuestionsSYS.Context;
using QuestionsSYS.Models;

namespace QuestionsSYS.Identity
{
    public class AuthService
    {
        public static void LoginStateSave(string user_id)
        {
            DatabaseContexts db = new DatabaseContexts();

            AuthHistory ah = db.auth_history.Where(s => s.login_date.Day == DateTime.Now.Day && s.login_date.Month == s.login_date.Month && s.login_date.Year == DateTime.Now.Year).FirstOrDefault();
            if (ah == null)
            {
                AuthHistory ah_ = new AuthHistory
                {
                    login_date = DateTime.Now,
                    user_id = user_id
                };
                db.auth_history.Add(ah_);
                db.SaveChanges();
            }
        }

        public static void LogoutStateSave(string user_id)
        {
            DatabaseContexts db = new DatabaseContexts();
            AuthHistory ah = db.auth_history.Where(s => s.login_date.Day == DateTime.Now.Day && s.login_date.Month == s.login_date.Month && s.login_date.Year == DateTime.Now.Year).FirstOrDefault();
            
            if(ah != null) {
                ah.logout_date = DateTime.Now;
                db.SaveChanges();            
            }
        }
    }
}