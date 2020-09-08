using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using System;

[assembly: OwinStartup(typeof(QuestionsSYS.App_Start.Startup))]

namespace QuestionsSYS.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Pages/Login")
            });

        }
    }
}
