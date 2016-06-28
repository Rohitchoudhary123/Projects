using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NAGGLE.Web.Models;
using NAGGLE.Domain.Model;
using NAGGLE.Service.Interfaces;
using Microsoft.AspNet.Identity;
using ExpressMapper;
using NAGGLE.Common.ServiceResponse;
//using NAGGLE.Domain.PackageType;
//using NAGGLE.Web.Helper;
using NAGGLE.Common.Enums;
using NAGGLE.Common.Utility;
using NAGGLE.Web.Helper;
namespace NAGGLE.Web.Controllers
{
    [AuthorizeUser]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;

        private readonly IProductService _productService;

        public UserController(IAccountService AccountService, IProductService ProductService)
            : base()
        {
            _accountService = AccountService;

            _productService = ProductService;
        }


        #region Dashboard

        public ActionResult Dashboard()
        {
            return View("~/Views/User/Dashboard/Index.cshtml");
        }
        #endregion

        #region Account

        public ActionResult Account()
        {
            return View("~/Views/User/Account/Index.cshtml");
        }
        /// <summary>
        /// Getting Account List
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAccountList()
        {

            var accountViewModel = new List<AccountViewModel>();
            var accountModel = new List<AccountModel>();
            var categoryViewModel = new List<TreeViewControlModel>();
            var accountTypeViewModel = new List<AccountTypeViewModel>();
            var countryViewModel = new List<CountryViewModel>();
            var stateViewModel = new List<StateViewModel>();
            var categories = _accountService.GetAllParentCategories().ToList();
            var accountTypes = _accountService.GetAllAccountTypes().ToList();

            var countries = _accountService.GetAllCountries().Result;
            var states = _accountService.GetStatesByCountryId(231).Result;


            Mapper.Map(accountTypes, accountTypeViewModel);
            Mapper.Map(categories, categoryViewModel);
            Mapper.Map(countries, countryViewModel);
            Mapper.Map(states, stateViewModel);

            accountModel = _accountService.GetAccountList();
            if (accountModel != null && accountModel.Count > 0)
            {
                Mapper.Map(accountModel, accountViewModel);
                for (int i = 0; i < accountModel.Count; i++)
                {
                    Mapper.Map(accountModel[i].AccountAccountTypeModel, accountViewModel[i].AccountAccountTypes);

                    Mapper.Map(accountModel[i].AccountCategoryModel, accountViewModel[i].AccountCategories);

                    Mapper.Map(accountModel[i].AccountCompanyModel, accountViewModel[i].AccountCompanies);

                    Mapper.Map(accountModel[i].AccountUserModel, accountViewModel[i].AccountUsers);

                }

                return Json(new { Success = true, Accounts = accountViewModel, AccountTypes = accountTypeViewModel, Categories = categoryViewModel, Countries = countryViewModel, States = stateViewModel, Message = "Successful" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, Accounts = accountViewModel, AccountTypes = accountTypeViewModel, Categories = categoryViewModel, Countries = countryViewModel, States = stateViewModel, Message = "Accounts were not found" }, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetChildCategory(string CategoryId)
        {
            List<TreeViewControlModel> categoryViewModel = new List<TreeViewControlModel>();
            List<CategoryModel> categoryModel = new List<CategoryModel>();
            categoryModel = _accountService.GetChildCategory(CategoryId.Decrypt());
            Mapper.Map(categoryModel, categoryViewModel);
            return Json(categoryViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// View Specific Account Into Account List
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAccountById(string accountId)
        {
            var accountViewModel = new AccountViewModel();
            var contacts = new List<UserViewModel>();
            //var response = new ServiceResponse();

            var accountModel = _accountService.GetAccountById(accountId.Decrypt());

            Mapper.Map(accountModel, accountViewModel);

            Mapper.Map(accountModel.AccountUserModel, accountViewModel.AccountUsers);

            for (int i = 0; i < accountModel.AccountUserModel.Count; i++)
            {
                accountViewModel.AccountUsers[i].UserViewModel = new UserViewModel();

                Mapper.Map(accountModel.AccountUserModel.ToList()[i].UserModel, accountViewModel.AccountUsers[i].UserViewModel);
                contacts.Add(accountViewModel.AccountUsers[i].UserViewModel);
            }
            Mapper.Map(accountModel.CountryModel, accountViewModel.CountryType);
            Mapper.Map(accountModel.AccountAccountTypeModel, accountViewModel.AccountAccountTypes);
            Mapper.Map(accountModel.AccountCategoryModel, accountViewModel.AccountCategories);
            Mapper.Map(accountModel.AccountCompanyModel, accountViewModel.AccountCompanies);


            if (accountViewModel != null)
            {
                return Json(new { Account = accountViewModel, Contacts = contacts, Success = true }, JsonRequestBehavior.AllowGet);

            }
            else { return Json(new { Account = accountViewModel, Success = false, Message = "No Account" }, JsonRequestBehavior.AllowGet); }




        }


        /// <summary>
        /// Add and Update Account Detail
        /// </summary>
        /// <param name="accountViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditAccount(AccountViewModel accountViewModel)
        {
            var accountModel = new AccountModel();
            Mapper.Map(accountViewModel, accountModel);
            Mapper.Map(accountViewModel.CountryType, accountModel.CountryModel);
            Mapper.Map(accountViewModel.AccountAccountTypes, accountModel.AccountAccountTypeModel);
            Mapper.Map(accountViewModel.AccountCategories, accountModel.AccountCategoryModel);
            Mapper.Map(accountViewModel.AccountCompanies, accountModel.AccountCompanyModel);

            //for (int i = 0; i < accountViewModel.AccountTypes.Count; i++)
            //{ 
            //    accountModel.AccountAccountTypeModel
            //}

            var result = new ServiceResponse();
            if (accountModel.AccountId > 0)
            {
                accountModel.AccountCategoryModel.ForEach(m => m.AccountId = accountModel.AccountId);
                accountModel.AccountAccountTypeModel.ForEach(m => m.AccountId = accountModel.AccountId);

                result = _accountService.UpdateAccountDetail(accountModel);
            }
            else
            {
                result = _accountService.SaveAccountDetail(accountModel);
            }
            AccountModel result1 = (AccountModel)result.Data;
            Mapper.Map(result1, accountViewModel);
            result.Data = accountViewModel;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete Account By Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public ActionResult DeleteAccount(string accountId)
        {
            bool response = _accountService.DeleteAccountById(accountId.Decrypt());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FilterAccounts(FilterParameter searchParam)
        {
            var accountModel = new List<AccountModel>();
            var accountViewModel = new List<AccountViewModel>();
            accountModel = _accountService.AccountFilter(searchParam);
            Mapper.Map(accountModel, accountViewModel);
            return Json(new { Success = true, Accounts = accountViewModel, Message = "Successful" }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Contacts

        public ActionResult Contact()
        {
            return View("~/Views/User/Account/Contact/Index.cshtml");
        }
        [HttpPost]
        public JsonResult GetContactList()
        {
            var contactViewModel = new List<UserViewModel>();
            var accountViewModel = new List<AccountViewModel>();
            var countryViewModel = new List<CountryViewModel>();
            var stateViewModel = new List<StateViewModel>();
            var countries = _accountService.GetAllCountries().Result;
            var states = _accountService.GetStatesByCountryId(231).Result;

            var accounts = _accountService.GetAccountList();
            var contactModel = _accountService.GetContactList();
            Mapper.Map(countries, countryViewModel);
            Mapper.Map(accounts, accountViewModel);
            Mapper.Map(contactModel, contactViewModel);
            Mapper.Map(states, stateViewModel);
            //for (int i = 0;i<contactModel.Count ;i++)
            //{

            //    contactViewModel[i].AccountId = contactModel[i].AccountId;
            //}




            return Json(new { Contacts = contactViewModel, Accounts = accountViewModel, Countries = countryViewModel, States = stateViewModel, Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddEditContact(UserViewModel userViewModel)
        {
            var userModel = new UserModel();
            Mapper.Map(userViewModel, userModel);
            var result = _accountService.AddEditContact(userModel, userModel.AccountId);
            userModel = (UserModel)result.Data;
            Mapper.Map(userModel, userViewModel);
            result.Data = userViewModel;
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeleteContact(string userId)
        {
            //   var response = false;
            bool response = _accountService.DeleteContactByUserId(userId.Decrypt());

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /*Filter Contacts By AccountId*/
        [HttpPost]
        public JsonResult FilterContactsByAccount(FilterParameter searchParam)
        {
            var userViewModel = new List<UserViewModel>();
            var userModel = _accountService.GetContactList(searchParam);
            Mapper.Map(userModel, userViewModel);
            return Json(userViewModel, JsonRequestBehavior.AllowGet);
        }
        //return Json(new { Success = true, Contacts = userViewModel, Message = "Successful" }, JsonRequestBehavior.AllowGet);
        public JsonResult GetContactById(string Id)
        {
            UserViewModel contactViewModel = new UserViewModel();
            UserModel contactModel = _accountService.GetContactById(Id.Decrypt());
            Mapper.Map(contactModel, contactViewModel);
            return Json(contactViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Attribute

        public ActionResult Attribute()
        {
            return View("~/Views/User/Products/Attribute/Index.cshtml");
        }

        public JsonResult GetAttributeList()
        {
            var attributeViewModel = new List<AttributeViewModel>();
            var featureTypeViewModel = new List<FeatureTypeViewModel>();
            var categoryViewModel = new List<TreeViewControlModel>();

            var attributeList = _productService.GetAttributeList();
            var featureTypes = _productService.AllFeatureTypes();
            var categories = _accountService.GetAllParentCategories().ToList();


            Mapper.Map(attributeList, attributeViewModel);
            for (int i = 0; i < attributeList.Count; i++)
            {
                Mapper.Map(attributeList[i].AttributeCategoryModel, attributeViewModel[i].AttributeCategories);
            }
            Mapper.Map(featureTypes, featureTypeViewModel);
            Mapper.Map(categories, categoryViewModel);

            return Json(new { Attributes = attributeViewModel, FeatureTypes = featureTypeViewModel, Categories = categoryViewModel, Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAttributeById(string eAttributeId)
        {

            int attributeId = eAttributeId != "" ? Common.Utility.Helper.Decrypt(eAttributeId) : 0;

            var attributeViewModel = new AttributeViewModel();
            if (attributeId > 0)
            {
                var attribute = _productService.GetAttributeById(attributeId);

                Mapper.Map(attribute, attributeViewModel);
                Mapper.Map(attribute.AttributeCategoryModel, attributeViewModel.AttributeCategories);
            }

            return Json(new { Attribute = attributeViewModel, Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult AddEditAttribute(AttributeViewModel attributeViewModel)
        {

            var message = attributeViewModel.AttributeId != null ? "Updated" : "Saved";

            var attributeModel = new AttributeModel();
            Mapper.Map(attributeViewModel, attributeModel);
            Mapper.Map(attributeViewModel.AttributeCategories, attributeModel.AttributeCategoryModel);

            var attribute = _productService.AddEditAttribute(attributeModel);



            Mapper.Map(attribute, attributeViewModel);
            Mapper.Map(attribute.AttributeCategoryModel, attributeViewModel.AttributeCategories);


            return Json(new { Attribute = attributeViewModel, Success = true, Message = message }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult DeleteSingleAttribute(string eAttributeId)
        //{
        //    int attributeId = Common.Utility.Helper.Decrypt(eAttributeId);
        //    var attribute = _productService.DeleteAttribute(attributeId);

        //    return Json(new { Attribute = attribute, Success = true, Message = "Delete SuccessFully" }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult DeleteSelectedAttribute(List<AttributeViewModel> selectedAttributes)
        {
            var attributeModelList = new List<AttributeModel>();
            Mapper.Map(selectedAttributes, attributeModelList);
            for (int i = 0; i < attributeModelList.Count; i++)
            {
                Mapper.Map(selectedAttributes[i].AttributeCategories, attributeModelList[i].AttributeCategoryModel);
            }

            var response = _productService.DeleteMultipleAttribute(attributeModelList);


            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateAttributeStatus(AttributeViewModel attributeViewModel)
        {
            var attributeModel = new AttributeModel();

            Mapper.Map(attributeViewModel, attributeModel);
            var att = _productService.UpdateAttributeStatus(attributeModel);
            Mapper.Map(att, attributeViewModel);


            return Json(attributeViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Products
        public ActionResult Product()
        {
            return View("~/Views/User/Products/Index.cshtml");
        }


        public JsonResult FilterProducts(FilterParameter searchParam)
        {
            var productViewModel = new List<ProductViewModel>();
           
            var products = _productService.FilterProducts(searchParam);
            
            Mapper.Map(products, productViewModel);
           
            return Json(new { Success = true, Products = productViewModel,Message="Success"}, JsonRequestBehavior.AllowGet);
        }

        // public JsonResult FilterAccounts(FilterParameter searchParam)
        //  {
        //    var accountModel = new List<AccountModel>();
        //    var accountViewModel = new List<AccountViewModel>();
        //    accountModel = _accountService.AccountFilter(searchParam);
        //    Mapper.Map(accountModel, accountViewModel);
        //    return Json(new { Success = true, Accounts = accountViewModel, Message = "Successful" }, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult GetProductList()
        {
            var productViewModel = new List<ProductViewModel>();
            var categoryViewModel = new List<TreeViewControlModel>();
            var divisionViewModel = new List<DivisionViewModel>();
            var availabilityViewModel = new List<AvailabilityViewModel>();
            var productClassViewModel = new List<ProductClassViewModel>();
            var accountViewModel = new List<AccountViewModel>();

            var products = _productService.GetProductList();
            var categories = _accountService.GetAllParentCategories().ToList();
            var divisions = _productService.GetAllDivisions();
            var availablities = _productService.GetAllAvailablities();
            var productClasses = _productService.GetAllProductClasses();
            var accounts = _accountService.GetAccountList();

            Mapper.Map(products, productViewModel);
            Mapper.Map(categories, categoryViewModel);
            Mapper.Map(divisions, divisionViewModel);
            Mapper.Map(availablities, availabilityViewModel);
            Mapper.Map(productClasses, productClassViewModel);
            Mapper.Map(accounts, accountViewModel);

            return Json(new { Success = true, Products = productViewModel, Categories = categoryViewModel, Divisions = divisionViewModel, Availabilities = availabilityViewModel, ProductClasses = productClassViewModel, Accounts = accountViewModel }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAttributeByCategoryId(List<TreeViewControlModel> categories)
        {
            var attributeViewModel = new List<AttributeViewModel>();
            var intIds=new List<int>();
            if (categories != null && categories.Count > 0)
            {
                intIds = categories.Select(m => m.Id.Decrypt()).ToList();
                var attributes = _productService.GetAttributeByCategoryId(intIds);

                Mapper.Map(attributes, attributeViewModel);
                //foreach (var item in categoryIds)
                //{
                //    intIds.Add(item.CategoryId.Decrypt());

                //}
                // List<string> by = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(categoryIds);

              
                //int categoryid = eCategoryid != null ? eCategoryid.Decrypt() : 0;
                //int categoryid = 0;
               
            }

            return Json(new { Success = true, CategoryAttributes = attributeViewModel }, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetProductById(string eProductId)
        {
            var productViewModel = new ProductViewModel();
            int productId = eProductId != null ? eProductId.Decrypt() : 0;

            var productModel = _productService.GetProductById(productId);
            if (productModel != null)
                Mapper.Map(productModel, productViewModel);
            productViewModel.ProductClassName = productModel.ProductClassModel.Name;

            Mapper.Map(productModel.ProductAccountPriceModel, productViewModel.ProductAccountPrices);

            for (int i = 0; i < productModel.ProductAccountPriceModel.Count; i++)
            {
                productViewModel.ProductAccountPrices[i].Account = new AccountViewModel();
                Mapper.Map(productModel.ProductAccountPriceModel[i].AccountModel, productViewModel.ProductAccountPrices[i].Account);
            }

            Mapper.Map(productModel.ProductAvailabilityModel, productViewModel.ProductAvailabilities);
            for (int i = 0; i < productModel.ProductAvailabilityModel.Count; i++)
            {
                productViewModel.ProductAvailabilities[i].Availability = new AvailabilityViewModel();
                productViewModel.ProductAvailabilities[i].Name = productModel.ProductAvailabilityModel[i].AvailabilityModel.Name;
                //Mapper.Map(productModel.ProductAvailabilityModel[i].AvailabilityModel, productViewModel.ProductAvailabilities[i].Availability);
            }
            Mapper.Map(productModel.ProductPricingModel, productViewModel.ProductPrices);
            Mapper.Map(productModel.ProductDivisionModel, productViewModel.ProductDivisions);
            Mapper.Map(productModel.ProductCategoryModel, productViewModel.ProductCategories);
            Mapper.Map(productModel.ProductAttributeModel, productViewModel.ProductAttributes);

            return Json(new { Success = true, Product = productViewModel }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddEditProduct(ProductViewModel productViewModel)
        {
            var response = new ServiceResponse();
            var productModel = new ProductModel();
            Mapper.Map(productViewModel, productModel);
            Mapper.Map(productViewModel.ProductAccountPrices, productModel.ProductAccountPriceModel);
            Mapper.Map(productViewModel.ProductAvailabilities, productModel.ProductAvailabilityModel);
            Mapper.Map(productViewModel.ProductPrices, productModel.ProductPricingModel);
            Mapper.Map(productViewModel.ProductDivisions, productModel.ProductDivisionModel);
            Mapper.Map(productViewModel.ProductCategories, productModel.ProductCategoryModel);
            Mapper.Map(productViewModel.ProductAttributes, productModel.ProductAttributeModel);

            if (productModel.ProductId > 0)
            {
                //productModel.ProductDivisionModel.ForEach(m => m.ProductId = productModel.ProductId);
                productModel.ProductAccountPriceModel.ForEach(m => m.ProductId = productModel.ProductId);
                //productModel.ProductPricingModel.ForEach(m => m.ProductId = productModel.ProductId);
                productModel.ProductAvailabilityModel.ForEach(m => m.ProductId = productModel.ProductId);

                productModel.ProductPricingModel.ForEach(m => m.ProductId = productModel.ProductId);
                response = _productService.UpdateProduct(productModel);
                productModel = (ProductModel)response.Data;
            }
            else
            {
                response = _productService.SaveProduct(productModel);
                productModel = (ProductModel)response.Data;

            }
            if (productModel != null)
            {
                Mapper.Map(productModel, productViewModel);
                Mapper.Map(productModel.ProductAccountPriceModel, productViewModel.ProductAccountPrices);
                Mapper.Map(productModel.ProductAvailabilityModel, productViewModel.ProductAvailabilities);
                Mapper.Map(productModel.ProductPricingModel, productViewModel.ProductPrices);
                Mapper.Map(productModel.ProductDivisionModel,productViewModel.ProductDivisions);
                Mapper.Map(productModel.ProductCategoryModel, productViewModel.ProductCategories);
                Mapper.Map(productModel.ProductAttributeModel, productViewModel.ProductAttributes);
            }

            return Json(new { Product = productViewModel, Message = response.Message, Success = response.Success }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProduct(string eProductId )
        {

            int productId = eProductId != null ? eProductId.Decrypt(): 0;

            var result = _productService.DeleteProduct(productId);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /*Update Status on ProductList Page*/
        public JsonResult UpdateProductStatus(ProductViewModel productViewModel)
        {
            var productModel = new ProductModel();

            Mapper.Map(productViewModel, productModel);
            var updateStatus = _productService.UpdateProductStatus(productModel);
            Mapper.Map(updateStatus, productViewModel);


            return Json(productViewModel, JsonRequestBehavior.AllowGet);
        }



        #endregion



        /// <summary>
        /// Getting States By Country Id
        /// </summary>
        /// <param name="dCountryId"></param>
        /// <returns></returns>

        public JsonResult GetStatesById(string CountryId)
        {
            int eCountryId = Convert.ToInt16(CountryId);
            List<StateViewModel> stateViewModel = new List<StateViewModel>();
            var states = _accountService.GetStatesByCountryId(eCountryId).Result;
            Mapper.Map(states, stateViewModel);
            var success = (states.Count > 0) ? true : false;
            return Json(new { Success = success, States = stateViewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupplierDashBoard()
        {
            return View();
        }
        public JsonResult SearchCategories(string searchText)
        {
            var Categories = _accountService.SearchCategories(searchText);
            var categoryViewModel = new List<AutoCompleteViewModel>();
            Mapper.Map(Categories, categoryViewModel);
            return Json(categoryViewModel, JsonRequestBehavior.AllowGet);
        }


    }
}

