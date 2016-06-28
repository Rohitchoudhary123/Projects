using NAGGLE.DAL.Interfaces;
using NAGGLE.Service.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using System.Security.Claims;

namespace NAGGLE.Service.Services
{
    public abstract class AbstractService : IDisposable
    {
        protected IUnitofWork _unitOfWork;
        protected ApplicationRoleManager _roleManager;
        protected ApplicationUserManager _userManager;
        protected ApplicationSignInManager _signInManager;
        public AbstractService()
        {

        }
        public AbstractService(IUnitofWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        protected ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        protected string GetLoginUserName()
        {
            return SignInManager.AuthenticationManager.User.Identity.GetUserName();
        }

        protected string GetLoginUserEmail()
        {
            return SignInManager.AuthenticationManager.User.Identity.Name;
        }
        protected int GetLoginUserId()
        {
            return int.Parse(SignInManager.AuthenticationManager.User.Identity.GetUserId());
        }

        protected int GetLoginUserCompanyId()
        {
            var cp = ClaimsPrincipal.Current.Identities.First();
            var companyId = cp.Claims.FirstOrDefault(c => c.Type == "CompanyId").Value;

            return Convert.ToInt32(companyId);
        }
    }
}
