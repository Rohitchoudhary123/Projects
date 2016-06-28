using Microsoft.AspNet.Identity;
using NAGGLE.Common.ServiceResponse;
using NAGGLE.DAL.Entities;
using NAGGLE.DAL.Interfaces;

using NAGGLE.Domain.Model;
using NAGGLE.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper;
using NAGGLE.Common.Constants;
using System.Web.Security;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using NAGGLE.Common.Enums;
using System.Security.Claims;
using System.Net;

namespace NAGGLE.Service.Services
{


    public class AuthenticationService : AbstractService, IAuthenticationService
    {
        public AuthenticationService(IUnitofWork unitOfWork)
            : base(unitOfWork)
        {
        }

        /// <summary>
        /// Create New User
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="isThirdParty"></param>
        /// <returns></returns>
        public Task<AuthenticationServiceResponse> CreateUser(UserModel userModel)
        {
            var existUser = UserManager.FindByEmail(userModel.Email);
            var userType =Convert.ToInt32(UserType.BusinessUser);
            IdentityResult result = new IdentityResult();
            if (existUser == null)
            {
                int companyId= CreateCompany(userModel.CompanyModel.Name);  // Creating Company and it returns Company Id
                userModel.CompanyId = companyId;
                ApplicationUser appUser = new ApplicationUser();
                User user = new User();
                Mapper.Map(userModel, appUser);
                appUser.UserName = userModel.Email;
                appUser.CreatedDate = DateTime.UtcNow;
                appUser.ShipmentType = Convert.ToInt32(ShipmentType.BusinessShipments);
                appUser.IsActive = true;
                try
                {
                    if (!string.IsNullOrEmpty(userModel.Password))
                    {
                        result = UserManager.Create(appUser,userModel.Password);  // Creating User
                    }
                    if (result.Succeeded)
                    {
                        _unitOfWork.GetContext().CreateUserRole(appUser.Id,userType); //Add User Role
                         _unitOfWork.Save();

                        Company companyParam = new Company();
                        companyParam.CompanyId = companyId;
                        companyParam.OwnerId = appUser.Id;
                        companyParam.CreatedBy = appUser.Id;
                        companyParam.UpdatedBy = appUser.Id;
                        UpdateCompany(companyParam);
                    }
                }
                catch (Exception ex) // if error occurs then reverting database entries
                {
                    if (!result.Succeeded)
                    {
                        DeleteCompany(companyId);
                    }
                    else {
                        appUser.IsDeleted = true;
                        UserManager.Update(appUser);
                    }
                    throw;
                }

                Mapper.Map(appUser, userModel);

                return Task.FromResult(new AuthenticationServiceResponse() { Data = userModel, Success = result.Succeeded,Message=result.Errors.FirstOrDefault() });

            }
            else // If User is deleted or, already exists in database
            {

                if (!existUser.IsDeleted && UserManager.HasPassword(existUser.Id))// && passwordExist)
                {
                    return Task.FromResult(new AuthenticationServiceResponse() { Data = userModel, Success = false, Message = "Sorry, it looks like <b>"+userModel.Email+" </b> belongs to an existing account" });
                }
                int companyId = CreateCompany(userModel.CompanyModel.Name);
                try
                {
                    existUser.FirstName = userModel.FirstName;
                    existUser.LastName = userModel.LastName;
                    existUser.UpdatedDate = null;
                    existUser.CreatedDate = DateTime.UtcNow;
                    existUser.IsActive = true;
                    existUser.IsDeleted = false;
                    existUser.CompanyId = companyId;
                    result = UserManager.Update(existUser);  // updating User
                    if (result.Succeeded)
                    {
                        _unitOfWork.GetContext().CreateUserRole(existUser.Id,userType); //Add User Role
                        _unitOfWork.Save();
                        //UserManager.AddToRole(existUser.Id,UserType.BusinessUser.ToString()); // Assigning Role Business because we are creatng a company account 
                      
                        Mapper.Map(existUser, userModel);

                        Company companyParam = new Company(); // Updating company again
                        companyParam.CompanyId = companyId;
                        companyParam.OwnerId = existUser.Id;
                        companyParam.CreatedBy = existUser.Id;
                        companyParam.UpdatedBy = existUser.Id;
                        UpdateCompany(companyParam);
                    }
                    return Task.FromResult(new AuthenticationServiceResponse() { Data = userModel, Success = result.Succeeded });
                }
                catch (Exception ex) // if error occurs then reverting database entries
                {
                    if (!result.Succeeded)
                    {
                        DeleteCompany(companyId);
                    }
                    else {
                        existUser.IsDeleted = true;
                        UserManager.Update(existUser);
                    }
                    throw;
                }
            }
        }
     
        /// <summary>
        /// It's use for Login to Registered User
        /// </summary>
        /// <param name="loginModel"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public AuthenticationServiceResponse SignInUser(UserModel userModel, bool isPersistent = false)
        {
            

            var user = UserManager.FindByEmail(userModel.UserName); //_unitOfWork.UserRepository.Get(filter => filter.Email == loginModel.UserName).FirstOrDefault();

            var role = "";
            role = user!=null?UserManager.GetRoles(user.Id).FirstOrDefault():"";//getting User Role eg:BusinessUser,User,Admin etc..

            if (user != null)
            {
                SignInStatus signStatus = SignInManager.PasswordSignIn(userModel.UserName, userModel.Password, false, //loginModel.RememberMe,
                   (!user.LockoutEnabled ? user.LockoutEnabled : UserManager.UserLockoutEnabledByDefault));

                int accessFailedCount = UserManager.GetAccessFailedCount(user.Id);
                int attemptsLeft = UserManager.MaxFailedAccessAttemptsBeforeLockout - accessFailedCount;
                bool halfAttemptExceed = accessFailedCount > (UserManager.MaxFailedAccessAttemptsBeforeLockout) / 2;
                if (signStatus != SignInStatus.Success && !halfAttemptExceed)
                {
                    return new AuthenticationServiceResponse() { Success = false, Message = UserLoginConstants.INVALID_LOGIN };
                }
                if (signStatus == SignInStatus.Success )//&& role == UserType.BusinessUser.ToString())// role=Only BusinessUser can be Login
                {
                    SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    SignInManager.AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, user.GenerateUserIdentityAsync(UserManager, true).Result);
                  //  return new AuthenticationServiceResponse() { Success = true, Message = "complete",Type=role};
                }
               
                //int accessFailedCount = UserManager.GetAccessFailedCount(user.Id);
                //int attemptsLeft = UserManager.MaxFailedAccessAttemptsBeforeLockout - accessFailedCount;
                //bool halfAttemptExceed = accessFailedCount > (UserManager.MaxFailedAccessAttemptsBeforeLockout) / 2;
                //if (signStatus != SignInStatus.Success && !halfAttemptExceed)
                //{
                //    return new AuthenticationServiceResponse() { Success = false, Message = UserLoginConstants.INVALID_LOGIN };
                //}
                //"Only Business User Can be login please contact with administrator"
                return new AuthenticationServiceResponse() { Success = true,Type=role, Message = "Login Successfully" };
            }
            else
            {
                return new AuthenticationServiceResponse() { Success = false, Message = UserLoginConstants.EMAIL_NOT_FOUND };
            }
        }
      
        /// <summary>
        /// It's use for sign out to Login user
        /// </summary>
        public void SignOut()
        {

            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public ExternalLoginInfo GetExternalLoginInfo()
        {
            return base.SignInManager.AuthenticationManager.GetExternalLoginInfo();
        }

        public AuthenticationServiceResponse ConfirmAccount(int userId, string code)
        {
            if (!string.IsNullOrEmpty(userId.ToString()) && !string.IsNullOrEmpty(code))
            {
                ApplicationUser user = UserManager.FindById(userId);
                if (user != null)
                {
                    string newCode = WebUtility.UrlDecode(code);
                    var result = UserManager.ConfirmEmail(userId, code);
                    if (!result.Succeeded)
                    {
                        return new AuthenticationServiceResponse() { Success = false, Message = "Licence Experienced" };
                    }

                    user.EmailConfirmed = true;
                    user.UpdatedDate = DateTime.Now;
                    UserManager.Update(user);
                    return new AuthenticationServiceResponse() { Success = true, Message = "Successfully" };
                }
            }
            return new AuthenticationServiceResponse() { Success = false, Message = "Invalid Code" };
        }

        public async Task<AuthenticationServiceResponse> SetPassword(int userId, string password)
        {
            AuthenticationServiceResponse response = new AuthenticationServiceResponse();
            UserManager.RemovePassword(userId);
            var result = await UserManager.AddPasswordAsync(userId, password);
            response.Success = result.Succeeded;
            response.Message = result.Errors.FirstOrDefault();

            var userInfo = UserManager.FindById(userId);//getting userInfo
            //userInfo.PasswordHash=password;
            response.Message= userInfo.UserName;
            
            return response;
        }




        #region Company

        /// <summary>
        /// It's use for save company info and it return company Id
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        private int CreateCompany(string companyName)
        {
            Company company = new Company();
            company.Name = companyName;
           // company.CreatedDate = DateTime.UtcNow;
             _unitOfWork.companyRepository.Insert(company);

            _unitOfWork.Save();
            return company.CompanyId;
        }
      
        /// <summary>
        /// Modify Company Info
        /// </summary>
        /// <param name="companyParam"></param>
        private void UpdateCompany(Company companyParam)
        {

            Company company = _unitOfWork.companyRepository.Get(c => c.CompanyId == companyParam.CompanyId).Single();
            company.Name = string.IsNullOrEmpty(companyParam.Name) ? company.Name : companyParam.Name;
            company.OwnerId = companyParam.OwnerId;
            company.CreatedBy = companyParam.CreatedBy;
            company.UpdatedBy = companyParam.UpdatedBy;
            company.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.companyRepository.Update(company);
            _unitOfWork.Save();
        }
      
        /// <summary>
        /// Delete Existing Company Info
        /// </summary>
        /// <param name="companyId"></param>
        private void DeleteCompany(int companyId)
        {
            _unitOfWork.companyRepository.Delete(companyId);
            _unitOfWork.Save();
        }

        #endregion


    }
}

