using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NAGGLE.Web.Models;
using NAGGLE.Domain.Model;
using ExpressMapper;
using NAGGLE.Common.ServiceResponse;
using NAGGLE.Service.Interfaces;
using NAGGLE.Common.Utility;

namespace NAGGLE.Web.Controllers
{
   
    public class AccountController : Controller
    {

        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService AuthenticationService)
            : base()
        {
            _authenticationService = AuthenticationService;
        }


        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SignUp(UserViewModel userViewModel)
         {
            if (ModelState.IsValid && userViewModel!=null)
            {
                UserModel userModel = new UserModel();
                Mapper.Map(userViewModel, userModel);
                userModel.FirstName = userViewModel.FullName;                
                userModel.CompanyModel.Name= userViewModel.CompanyName;
                

                var response = await _authenticationService.CreateUser(userModel);

                if (response.Success == true)
                {
                   
                    //userModel.UserName = userModel.Email;
                    //userModel.Password = userModel.Password;
                    AuthenticationServiceResponse loginResponse = _authenticationService.SignInUser(userModel);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json(new {Message="ModelState IsVaid is False",Success=false}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Login Existing User
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SignIn(LoginViewModel loginViewModel)
        {
            UserModel userModel = new UserModel();
            Mapper.Map(loginViewModel, userModel);
            AuthenticationServiceResponse response = _authenticationService.SignInUser(userModel, true);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        /// <summary>
        /// Sign Out Existing User
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmAccount(string userId, string code)
        {
            int Id = string.IsNullOrEmpty(userId) ? 0 : userId.Decrypt();
            var response = _authenticationService.ConfirmAccount(Id, code);
            if (!response.Success)
            {

                ViewBag.Message = response.Message;
                return RedirectToAction("Expired", "Error");
            }
            ViewBag.Message = response.Message;
            ViewBag.UserId = Id;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> SetPassword(ResetPasswordViewModel model)
        {
            var result = await _authenticationService.SetPassword(model.UserId, model.Password);
            if (!result.Success)
            {
                ModelState.AddModelError("Error", result.Message);
                return Json(new { success = false, message = result.Message }, JsonRequestBehavior.AllowGet);
            }
            var userModel = new UserModel {UserName=result.Message,Password=model.Password};
          var loginRewsponse=  _authenticationService.SignInUser(userModel);

          return Json(new { User = loginRewsponse, success = true, message = "Successfully save password" }, JsonRequestBehavior.AllowGet);
            

        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

       


    }
}