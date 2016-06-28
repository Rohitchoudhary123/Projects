using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAGGLE.DAL.Entities;
using NAGGLE.Domain.Model;

namespace NAGGLE.Service.ExpressMapper
{
    public static class ExpressMapperBusinessProfile
    {
        public static void RegisterMapping()
        {
            #region EntityToDomain

            Mapper.Register<Account, AccountModel>();
            Mapper.Register<Company, CompanyModel>();
            Mapper.Register<User, UserModel>()
                .Member(dest => dest.StateName, src => src.State.Name)
                .Member(dest => dest.CountryModel, src => src.Country)
                .Member(dest => dest.BusinessName, src => src.AccountContact.FirstOrDefault().Account != null ? src.AccountContact.FirstOrDefault().Account.BusinessName : null)
                .Member(dest => dest.AccountId, src => src.AccountContact != null ? src.AccountContact.FirstOrDefault().AccountId : 0);
            Mapper.Register<Country, CountryModel>();
            Mapper.Register<Category, CategoryModel>()
                .Member(dest => dest.AccountCategoryModel, src => src.AccountCategory);
            
            Mapper.Register<NAGGLE.DAL.Entities.Attribute, AttributeModel>();
            Mapper.Register<FeatureType, FeatureTypeModel>();
            Mapper.Register<Product,ProductModel>();

            Mapper.Register<Division, DivisionModel>();
            Mapper.Register<Availability, AvailabilityModel>();
            Mapper.Register<ProductClass, ProductClassModel>();

            
            #region MapperTable
            Mapper.Register<AccountContact, AccountUserModel>();
            Mapper.Register<AccountCompany, AccountCompanyModel>();
            Mapper.Register<AccountCategory, AccountCategoryModel>()
                .Member(dest => dest.Name, src => src.Category.Name);
            Mapper.Register<AccountAccountType, AccountAccountTypeModel>()
                .Member(dest => dest.Name, src => src.AccountType.Name);
            Mapper.Register<AttributeCategory, AttributeCategoryModel>()
                .Member(dest => dest.Name, src => src.Category != null ? src.Category.Name : null);
                
            Mapper.Register<ProductAccountPrice, ProductAccountPriceModel>();
            Mapper.Register<ProductAvailability, ProductAvailabilityModel>();
            Mapper.Register<ProductDivision, ProductDivisionModel>()
                .Member(dest => dest.Name, src => src.Division.Name);
            Mapper.Register<ProductPricing, ProductPricingModel>();
            Mapper.Register<ProductCategory, ProductCategoryModel>()
                      .Member(dest => dest.Name, src => src.Category.Name); 
               
            #endregion






            #endregion

            #region DomainToEntity

            Mapper.Register<AccountModel, Account>();
            Mapper.Register<UserModel, User>()
                    .Member(dest => dest.UserName, src => src.Email);
            Mapper.Register<CountryModel, Country>();
            Mapper.Register<AccountTypeModel, AccountType>();
          
            Mapper.Register<AttributeModel,NAGGLE.DAL.Entities.Attribute>();
            Mapper.Register<FeatureTypeModel,FeatureType>();
            Mapper.Register<ProductModel, Product>();
            Mapper.Register<DivisionModel,Division>();
            Mapper.Register<AvailabilityModel, Availability>();
            Mapper.Register<ProductClassModel,ProductClass>();
            #region MapperTable
            Mapper.Register<AccountCategoryModel, AccountCategory>();
            Mapper.Register<AccountAccountTypeModel, AccountAccountType>();
            Mapper.Register<AccountCompanyModel, AccountCompany>();
            Mapper.Register<AccountUserModel, AccountContact>();
            Mapper.Register<AttributeCategoryModel,AttributeCategory>();

            Mapper.Register<ProductAccountPriceModel,ProductAccountPrice>();
            Mapper.Register<ProductAvailabilityModel,ProductAvailability>();
            Mapper.Register<ProductDivisionModel,ProductDivision>();
            Mapper.Register<ProductPricingModel,ProductPricing>();
            Mapper.Register<ProductCategoryModel,ProductCategory>();

            #endregion




            #endregion
        }
    }
}

