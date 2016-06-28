using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using NAGGLE.DAL.Entities.IddentityDBContext;
using NAGGLE.Service.Authentication;
using Microsoft.Owin.Security;
using NAGGLE.DAL.Entities;

namespace NAGGLE.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext<IdentityDBContext>(IdentityDBContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Index"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator
                        .OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                            validateInterval: TimeSpan.FromMinutes(5),
                            regenerateIdentityCallback: (manager, user)
                                => user.GenerateUserIdentityAsync(manager),
                                getUserIdCallback: (claim) => int.Parse(claim.GetUserId()))
                }
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.Properties["Microsoft.Owin.Security.Constants.DefaultSignInAsAuthenticationType"] = "ExternalCookie";

        }

    }
}