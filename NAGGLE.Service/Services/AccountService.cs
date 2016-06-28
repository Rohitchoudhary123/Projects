using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAGGLE.DAL.Entities;
using NAGGLE.DAL.Interfaces;
using NAGGLE.Service.Interfaces;
//using NAGGLE.Service.Services;


using ExpressMapper;
using NAGGLE.Domain.Model;
using Microsoft.AspNet.Identity;
using NAGGLE.Common.Enums;
using System.Data.Entity.Validation;
using NAGGLE.Common.ServiceResponse;
using NAGGLE.Common.Utility;
using System.Net;
using NAGGLE.Common.Constants;
using System.Data.SqlClient;

namespace NAGGLE.Service.Services
{
    public class AccountService : AbstractService, IAccountService
    {

        int currentLoginCompanyId = 0;
        int loginUserId = 0;
        public AccountService(IUnitofWork unitwork) :
            base(unitwork)
        {
            //currentLoginCompanyId = GetLoginUserCompanyId();

            //loginUserId = GetLoginUserId();
        }

        #region Accounts

        /// <summary>
        /// Get All Account
        /// </summary>
        /// <returns></returns>
        public List<AccountModel> GetAccountList()
        {
            loginUserId = GetLoginUserId();
            currentLoginCompanyId = GetLoginUserCompanyId();

            var accountModel = new List<AccountModel>();
            var accountUserModel = new List<AccountUserModel>();

            var accounts = _unitOfWork.accountRepository.GetAsync(m => m.AccountCompany.Where(x => x.CompanyId == currentLoginCompanyId).Count() > 0 && m.IsDeleted == false).Result;
            //var accounts = response.Result;
            Mapper.Map(accounts, accountModel);
            for (int i = 0; i < accounts.Count; i++)
            {
                Mapper.Map(accounts[i].AccountAccountType, accountModel[i].AccountAccountTypeModel);

                Mapper.Map(accounts[i].AccountCategory, accountModel[i].AccountCategoryModel);

                Mapper.Map(accounts[i].AccountCompany, accountModel[i].AccountCompanyModel);

                Mapper.Map(accounts[i].AccountContact, accountModel[i].AccountUserModel);

            }




            //Commented Code
            #region AccountContact
            //if (accounts != null && accounts.count > 0)
            //{
            //    for (int i = 0; i < accounts.count; i++)
            //    {
            //        mapper.map(accounts[i].account, accountcompanymodel[i].accountmodel);

            //    }
            //    foreach (var item in accountcompanymodel)
            //    {
            //        accountmodel.add(item.accountmodel);

            //    }


            //    for (int i = 0; i < accounts.count; i++)
            //    {
            //        mapper.map(accounts[i].user, accountmodel[i].usermodel);


            //        mapper.map(accounts[i].accountcontact, accountmodel[i].accountusermodel);

            //        for (int j = 0; j < accounts[i].accountcontact.count; j++)
            //        {

            //            accountmodel[i].accountusermodel[j].usermodel = new usermodel();


            //            mapper.map(accounts[i].accountcontact.tolist()[j].user, accountmodel[i].accountusermodel[j].usermodel);
            //        }

            //    }
            //}

            //var list = _unitOfWork.accountUserRespository.Get();

            // Mapper.Map(accounts, accountModel);
            #endregion

            return accountModel;
        }

        public List<AccountModel> AccountFilter(FilterParameter parameters)
        {
            currentLoginCompanyId = GetLoginUserCompanyId();

            var accountModel = new List<AccountModel>();

            // var accounts =new List<Account>();
            int AccountTypeId = parameters.AccountTypeId.Decrypt();
            int CategoryId = parameters.CategoryId.Decrypt();
            int AccountId = parameters.AccountId.Decrypt();
            var accounts = _unitOfWork.accountRepository.GetAsync(m => m.AccountCompany.Where(x => x.CompanyId == currentLoginCompanyId).Count() > 0
                 && m.IsDeleted == false && (AccountTypeId != 0 ? m.AccountAccountType.Where(x => x.AccountTypeId == AccountTypeId).Count() > 0 : true)
                 && (CategoryId != 0 ? m.AccountCategory.Where(x => x.CategoryId == CategoryId).Count() > 0 : true) 
                 && (AccountId != 0 ? m.AccountId == AccountId : true) 
                 && (parameters.SearchText != null ? (m.BusinessName.Contains(parameters.SearchText) || m.Email.Contains(parameters.SearchText)) : true)).Result;// && (parameters.AccountId!=null)? m.AccountId==parameters.AccountId:true).Result;

            Mapper.Map(accounts, accountModel);
            return accountModel;
        }


        /// <summary>
        /// Get Specific Account detail
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public AccountModel GetAccountById(int accountId)
        {
            var accountModel = new AccountModel();

            var account = _unitOfWork.accountRepository.Get(m => m.AccountId == accountId).FirstOrDefault();
            var countries = _unitOfWork.countryRepository.Get(m => m.CountryId == account.CountryId).FirstOrDefault();

            if (account != null)
            {
                Mapper.Map(account, accountModel);
                Mapper.Map(account.AccountContact, accountModel.AccountUserModel);
                for (int i = 0; i < account.AccountContact.Count; i++)
                {
                    accountModel.AccountUserModel[i].UserModel = new UserModel();
                    Mapper.Map(account.AccountContact.ToList()[i].User, accountModel.AccountUserModel[i].UserModel);

                    accountModel.AccountUserModel[i].UserModel.BusinessName = account.BusinessName;
                    accountModel.AccountUserModel[i].UserModel.AccountId = account.AccountId;
                }
                Mapper.Map(countries, accountModel.CountryModel);
                Mapper.Map(account.AccountAccountType, accountModel.AccountAccountTypeModel);
                Mapper.Map(account.AccountCategory, accountModel.AccountCategoryModel);
                Mapper.Map(account.AccountCompany, accountModel.AccountCompanyModel);
            }
            return accountModel;
        }

        /// <summary>
        /// Save Account Detail 
        /// </summary>
        /// <returns></returns>
        public ServiceResponse SaveAccountDetail(AccountModel accountModel)
        {
            var response = new ServiceResponse();
            IdentityResult result = new IdentityResult();
            currentLoginCompanyId = GetLoginUserCompanyId();
            loginUserId = GetLoginUserId();
            var existUser = UserManager.FindByEmail(accountModel.Email);
            if (existUser == null)
            {
                var userType = Convert.ToInt32(UserType.Supplier);
                var account = new Account();
                Mapper.Map(accountModel, account);
                Mapper.Map(accountModel.AccountAccountTypeModel, account.AccountAccountType);
                Mapper.Map(accountModel.AccountCategoryModel, account.AccountCategory);
                ApplicationUser appUser = new ApplicationUser();
                appUser.FirstName = accountModel.FirstName;
                appUser.LastName = accountModel.LastName;
                appUser.UserName = accountModel.Email;
                appUser.Email = accountModel.Email;
                appUser.CompanyId = currentLoginCompanyId;
                appUser.CreatedDate = DateTime.UtcNow;
                appUser.CreatedBy = loginUserId;
                appUser.IsActive = true;
                if (!string.IsNullOrEmpty(appUser.UserName))
                {
                    result = UserManager.Create(appUser);  // Creating User
                }
                if (result.Succeeded)
                {
                    account.OwnerId = appUser.Id;
                    account.CreatedBy = loginUserId;
                    var savedAccount = CreateAccount(account);
                    _unitOfWork.GetContext().CreateUserRole(appUser.Id, userType);
                    _unitOfWork.Save();

                    SendEmail(appUser, MailSubject.CONFIRM_ACCOUNT);

                    Mapper.Map(savedAccount, accountModel);
                }
                else { return new ServiceResponse() { Data = accountModel, Message = result.Errors.FirstOrDefault(), Success = false }; }
                Mapper.Map(appUser, accountModel.UserModel);

                return new ServiceResponse() { Data = accountModel, Message = "Saved", Success = true };
            }
            else
            {
                return new ServiceResponse() { Data = accountModel, Message = "Sorry, it looks like <b>" + accountModel.Email + " </b> belongs to an existing account", Success = false };
            }
        }

        /// <summary>
        /// Delete Specific Account By AccountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public bool DeleteAccountById(int accountId = 0)
        {
            try
            {
                var accountCategory = _unitOfWork.accountCategoryRespository.Get(c => c.AccountId == accountId);
                var accountAccountType = _unitOfWork.accountAccountTypeRespository.Get(c => c.AccountId == accountId);
                var accountCompany = _unitOfWork.accountCompanyRespository.Get(c => c.AccountId == accountId);
                var accountContact = _unitOfWork.accountContactRespository.Get(c => c.AccountId == accountId);
                var accountInfo = _unitOfWork.accountRepository.Get(c => c.AccountId == accountId);

                if (accountCategory.Count > 0)
                    _unitOfWork.accountCategoryRespository.DeleteAll(accountCategory);

                if (accountContact.Count > 0)
                    _unitOfWork.accountContactRespository.DeleteAll(accountContact);


                if (accountAccountType.Count > 0)
                    _unitOfWork.accountAccountTypeRespository.DeleteAll(accountAccountType);

                if (accountCompany.Count > 0)
                    _unitOfWork.accountCompanyRespository.DeleteAll(accountCompany);

                if (accountInfo.Count > 0)
                    _unitOfWork.accountRepository.Delete(accountId);

                _unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool DeleteContactByUserId(int userId = 0)
        {
            try
            {
                var accountContact = _unitOfWork.accountContactRespository.Get(c => c.UserId == userId);
                var contactInfo = _unitOfWork.userRespository.Get(c => c.Id == userId);

                if (accountContact.Count > 0)
                    _unitOfWork.accountContactRespository.DeleteAll(accountContact);

                if (contactInfo.Count > 0)
                    _unitOfWork.userRespository.Delete(userId);

                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public UserModel GetContactById(int userId)
        {
            UserModel contactModel = new UserModel();
            User contactInfo = _unitOfWork.userRespository.Get(x => x.Id == userId).FirstOrDefault();
            Mapper.Map(contactInfo, contactModel);
            return contactModel;
        }
        private Account CreateAccount(Account account)
        {
            currentLoginCompanyId = GetLoginUserCompanyId();
            var accountCompany = new AccountCompany();
            accountCompany.CompanyId = currentLoginCompanyId;

            account.AccountCompany = new List<AccountCompany> { accountCompany };

            _unitOfWork.accountRepository.Insert(account);
            _unitOfWork.Save();
            return account;
        }

        public ServiceResponse UpdateAccountDetail(AccountModel accountModel)
        {
            var account = new Account();
            // Mapper.Map(accountModel, account);
            Mapper.Map(accountModel.AccountAccountTypeModel, account.AccountAccountType);
            Mapper.Map(accountModel.AccountCategoryModel, account.AccountCategory);

            var accountInfo = _unitOfWork.accountRepository.Get(c => c.AccountId == accountModel.AccountId).FirstOrDefault();

            Mapper.Map(accountModel, accountInfo);

            var accountCategory = _unitOfWork.accountCategoryRespository.Get(c => c.AccountId == accountModel.AccountId);
            var accountAccountType = _unitOfWork.accountAccountTypeRespository.Get(c => c.AccountId == accountModel.AccountId);
            var accountCompany = _unitOfWork.accountCompanyRespository.Get(c => c.AccountId == accountModel.AccountId);


            //foreach (var item in account.AccountCategory)
            //{
            //    var result = accountCategory.Where(m => m.CategoryId == item.CategoryId);

            //    if (result.Count() == 0)
            //    {
            //        _unitOfWork.accountCategoryRespository.Insert(item);
            //        _unitOfWork.Save();
            //    }
            //    //else { _unitOfWork.accountAccountTypeRespository.Delete(item); }

            //    i++;
            //}
            //foreach (var item in accountCategory)
            //{
            //    var result = account.AccountCategory.Where(m => m.CategoryId == item.CategoryId);
            //    if (result.Count() == 0)
            //    {
            //        _unitOfWork.accountCategoryRespository.Delete(item);
            //        _unitOfWork.Save();
            //    }
            //}

            if (accountAccountType.Count > 0)
                _unitOfWork.accountAccountTypeRespository.DeleteAll(accountAccountType);
            if (accountCategory.Count > 0)
                _unitOfWork.accountCategoryRespository.DeleteAll(accountCategory);

            accountAccountType = account.AccountAccountType.ToList();
            accountCategory = account.AccountCategory.ToList();

            _unitOfWork.accountAccountTypeRespository.InsertAll(accountAccountType);
            _unitOfWork.accountCategoryRespository.InsertAll(accountCategory);

            _unitOfWork.accountRepository.Update(accountInfo);

            _unitOfWork.Save();
            Mapper.Map(accountInfo, accountModel);

            return new ServiceResponse { Message = "Update", Success = true, Data = accountModel };
        }

        public void SendEmail(ApplicationUser userInfo, string emailSubject)
        {
            string mailBody = string.Empty;
            string code = string.Empty;

            // mailBodyTemplate.UserName = userInfo.FirstName + " " + userInfo.LastName;

            MailHelper mailHelper = new MailHelper();
            mailHelper.ToEmail = userInfo.Email;


            if (emailSubject == MailSubject.CONFIRM_ACCOUNT)
            {
                code = UserManager.GenerateEmailConfirmationTokenAsync(userInfo.Id).Result.ToString();
                code = WebUtility.UrlEncode(code);

                string newCode = WebUtility.UrlDecode(code);

                string AccountLoginUrl = CommonFunction.GetConfirmAccountUrl(userInfo.Id.Encrypt(), code);

                mailHelper.Subject = MailSubject.CONFIRM_ACCOUNT.ToString();
                mailHelper.Body = CommonFunction.ConfigureConfirmAccountMailBodyDb(AccountLoginUrl); //GetEmailBody(AccountLoginUrl);
                //  mailHelper.Subject = emailTemplatesModel.SingleOrDefault(x => x.TemplateName == "ConfirmAccount").Subject;
                //  mailHelper.FromEmail = emailTemplatesModel.SingleOrDefault(x => x.TemplateName == "ConfirmAccount").FromAddress;
            }
            mailHelper.SendEmail();

        }

        #endregion

        #region Contacts

        public List<UserModel> GetContactList(FilterParameter searchParam = null)
        {
            currentLoginCompanyId = GetLoginUserCompanyId();
            var contactModel = new List<UserModel>();

            int AccountId = searchParam != null ? searchParam.AccountId.Decrypt() : 0;
            var contacts = searchParam == null ? _unitOfWork.userRespository.Get(m => m.AccountContact.Count > 0 && m.CompanyId == currentLoginCompanyId && m.IsDeleted == false) : _unitOfWork.userRespository.Get(m => m.AccountContact.Count > 0 && m.CompanyId == currentLoginCompanyId && m.IsDeleted == false && AccountId == 0 ? true : m.AccountContact.Where(ac => ac.AccountId == AccountId).ToList().Count > 0);

            Mapper.Map(contacts, contactModel);

            for (int i = 0; i < contacts.Count; i++)
            {
                for (int j = 0; j < contacts[i].AccountContact.Count; j++)
                {
                    contactModel[i].BusinessName = contacts[i].AccountContact.ToList()[j].Account.BusinessName;
                    contactModel[i].AccountId = contacts[i].AccountContact.ToList()[j].Account.AccountId;
                }
            }
            return contactModel;
        }

        public ServiceResponse AddEditContact(UserModel userModel, int accountId)
        {
            currentLoginCompanyId = GetLoginUserCompanyId();
            loginUserId = GetLoginUserId();
            IdentityResult result = new IdentityResult();
            var user = new User();
            var existUser = _unitOfWork.userRespository.Get(m => m.Email == userModel.Email && m.Id != userModel.Id).FirstOrDefault();
            if (userModel.Id > 0 && existUser == null)
            {

                user = _unitOfWork.userRespository.Get(c => c.Id == userModel.Id).FirstOrDefault();
                var accountContactInfo = _unitOfWork.accountContactRespository.Get(c => c.UserId == userModel.Id).FirstOrDefault();
                Mapper.Map(userModel, user);
                user.BusinessName = null;
                user.UpdatedBy = loginUserId;
                user.CompanyId = currentLoginCompanyId;
                if (accountContactInfo != null && accountContactInfo.AccountId != accountId)
                {
                    var accountContact = new AccountContact() { AccountId = accountId };
                    user.AccountContact = new List<AccountContact> { accountContact };
                }
                _unitOfWork.userRespository.Update(user);
            }
            else
            {
                var DeleteUser = UserManager.FindByEmail(userModel.Email);
                if (DeleteUser != null && existUser == null)
                {
                    DeleteUser.IsDeleted = false;
                    Mapper.Map(DeleteUser, user);
                    Mapper.Map(userModel, user);
                    user.UpdatedBy = loginUserId;
                    user.CompanyId = currentLoginCompanyId;
                    var accountContact = new AccountContact() { AccountId = accountId };
                    user.AccountContact = new List<AccountContact> { accountContact };
                    _unitOfWork.userRespository.Update(user);

                }
                else
                {
                    if (existUser == null)
                    {
                        Mapper.Map(userModel, user);

                        var accountContact = new AccountContact() { AccountId = accountId };
                        user.AccountContact = new List<AccountContact> { accountContact };
                        user.CreatedBy = loginUserId;
                        user.CompanyId = currentLoginCompanyId;
                        _unitOfWork.userRespository.Insert(user);
                    }
                    else
                    {
                        return new ServiceResponse { Data = userModel, Message = "Sorry, it looks like <b>" + userModel.Email + " </b> belongs to an existing account", Success = false };
                    }
                }
            }
            _unitOfWork.Save();
            Mapper.Map(user, userModel);
            return new ServiceResponse() { Data = userModel, Message = "Success", Success = true };
        }






        public ServiceResponse UpdateContact(UserModel userModel, int accountId)
        {

            return new ServiceResponse { Data = userModel, Message = "Sorry, it looks like <b>" + userModel.Email + " </b> belongs to an existing account", Success = false };
        }


        #endregion

        /*Get All Categories*/
        public List<CategoryModel> GetAllParentCategories()
        {
            var context = _unitOfWork.GetContext();
            List<CategoryModel> result = context.Database.SqlQuery<CategoryModel>("[GetAllParentCategories]").ToList();
            return result;
        }
        public List<CategoryModel> GetChildCategory(int CategoryId)
        {
            var context = _unitOfWork.GetContext();
            SqlParameter param1 = new SqlParameter("@Id", CategoryId);
            List<CategoryModel> result = context.Database.SqlQuery<CategoryModel>("[GetCategoriesById] @Id", param1).ToList();
            return result;
        }
        //public List<CategoryModel> GetAllCategories()
        //{
        //    var categoriesModel = new List<CategoryModel>();
        //    var categories = _unitOfWork.categoryRespository.Get();
        //    Mapper.Map(categories, categoriesModel);
        //    return categoriesModel;
        //}

        public List<AccountTypeModel> GetAllAccountTypes()
        {
            var accountTypeModel = new List<AccountTypeModel>();
            var accountTypes = _unitOfWork.accountTypeRespository.Get();
            Mapper.Map(accountTypes, accountTypeModel);
            return accountTypeModel;
        }
        //public List<CategoryModel> SearchCategories(string SearchText)
        //{
        //    var categoriesModel = new List<CategoryModel>();
        //    var categories = _unitOfWork.categoryRespository.Get(x => x.Name.Contains(SearchText) && x.IsSelectable);
        //    Mapper.Map(categories, categoriesModel);
        //    return categoriesModel;
        //}
        public List<AutoCompleteModel> SearchCategories(string SearchText)
        {
            try
            {
                var context = _unitOfWork.GetContext();
                SqlParameter param1 = new SqlParameter("@SearchString", SearchText);
                List<AutoCompleteModel> result = context.Database.SqlQuery<AutoCompleteModel>("[SearchCategoriesByText] @SearchString", param1).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Task<List<CountryModel>> GetAllCountries()
        {
            List<CountryModel> countriesModel = new List<CountryModel>();

            var countries = _unitOfWork.countryRepository.GetAsync();

            Mapper.Map(countries.Result, countriesModel);

            return Task.FromResult(countriesModel);
        }
        public Task<List<StateModel>> GetStatesByCountryId(int countryId)
        {
            List<StateModel> statesModel = new List<StateModel>();
            var states = _unitOfWork.stateRepository.GetAsync(x => x.CountryId == countryId && x.Code != null);

            Mapper.Map(states.Result, statesModel);

            return Task.FromResult(statesModel);
        }



        //private int CreateCompany(string companyName)
        //{
        //    Company company = new Company();
        //    company.Name = companyName;
        //    company.CreatedDate = DateTime.UtcNow;

        //    _unitOfWork.companyRepository.Insert(company);

        //    _unitOfWork.Save();
        //    return company.CompanyId;
        //}

        ///// <summary>
        ///// Modify Company Info
        ///// </summary>
        ///// <param name="companyParam"></param>
        //private void UpdateCompany(Company companyParam)
        //{

        //    Company company = _unitOfWork.companyRepository.Get(c => c.CompanyId == companyParam.CompanyId).Single();
        //    company.Name = string.IsNullOrEmpty(companyParam.Name) ? company.Name : companyParam.Name;
        //    company.OwnerId = companyParam.OwnerId;
        //    company.UpdatedBy = companyParam.UpdatedBy;
        //    _unitOfWork.companyRepository.Update(company);
        //    _unitOfWork.Save();
        //}

        ///// <summary>
        ///// Delete Existing Company Info
        ///// </summary>
        ///// <param name="companyId"></param>
        //private void DeleteCompany(int companyId)
        //{
        //    _unitOfWork.companyRepository.Delete(companyId);
        //    _unitOfWork.Save();
        //}



    }
}
