using NAGGLE.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NAGGLE.Common.Utility;
namespace NAGGLE.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public HomeController(IAuthenticationService authenticationService)
            : base()
        {
            _authenticationService = authenticationService;
        }
        
        public ActionResult Index(string userId = null, string code = null)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(code))
            {
                int Id = string.IsNullOrEmpty(userId) ? 0 : userId.Decrypt();
                var response = _authenticationService.ConfirmAccount(Id, code);
                if (!response.Success)
                {
                    ViewBag.IsConfirmed = false;
                    // ViewBag.Message = response.Message;
                    //return RedirectToAction("Expired", "Error");
                }
                else
                {
                    ViewBag.IsConfirmed = true;
                }
                ViewBag.UserId = Id;

            }
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "User");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}