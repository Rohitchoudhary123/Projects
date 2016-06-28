using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NAGGLE.Common.ServiceResponse;
using NAGGLE.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationServiceResponse> SetPassword(int userId, string password);
        AuthenticationServiceResponse ConfirmAccount(int userId, string code);
        //Create New User
        Task<AuthenticationServiceResponse> CreateUser(UserModel userModel);
        //Login Existing User
        AuthenticationServiceResponse SignInUser(UserModel userModel, bool isPersistent = false);
        //Logoff Existing User
        void SignOut();
      

      
    }
}
