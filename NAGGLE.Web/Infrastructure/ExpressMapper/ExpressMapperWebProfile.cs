using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NAGGLE.Domain.Model;
using NAGGLE.Web.Models;
using NAGGLE.Common.Utility;

namespace NAGGLE.Web.Infrastructure.ExpressMapper
{
    public static class ExpressMapperWebProfile
    {
        public static void RegisterMapping()
        {
            #region ViewToDomain

            Mapper.Register<AccountUserViewModel, AccountUserModel>()
             .Member(dest => dest.AccountId, src => src.AccountId.Decrypt())
                        .Member(dest => dest.UserId, src => src.UserId.Decrypt());
            Mapper.Register<AccountViewModel, AccountModel>()
                 .Member(dest => dest.AccountId, src => src.AccountId.Decrypt())
                 .Member(dest => dest.OwnerId, src => src.OwnerId != null ? src.OwnerId.Decrypt() : (int?)null)
                 .Member(dest => dest.StateId, src => src.StateId != null ? src.StateId.Decrypt() : (int?)null)
                 .Member(dest => dest.CountryId, src => src.CountryType.CountryId != null ? src.CountryType.CountryId.Decrypt() : (int?)null);
             Mapper.Register<AccountCompanyModel, AccountCompanyViewModel>()
            .Member(dest => dest.AccountCompanyId, src => src.AccountCompanyId.Encrypt())
            .Member(dest => dest.AccountId, src => src.AccountId.Encrypt())
            .Member(dest => dest.CompanyId, src => src.CompanyId.Encrypt());
            Mapper.Register<UserViewModel, UserModel>()
            .Member(dest => dest.AccountId, src => src.AccountId.Decrypt())
            .Member(dest => dest.Id, src => src.Id.Decrypt())
            .Member(dest => dest.CompanyId, src => src.CompanyId.Decrypt())
            .Member(dest => dest.StateId, src => src.StateId != null ? src.StateId.Decrypt() : (int?)null)
            .Member(dest => dest.CountryId, src => src.CountryType.CountryId.Decrypt());
            Mapper.Register<AccountTypeViewModel, AccountTypeModel>()
                .Member(dest => dest.AccountTypeId, src => src.AccountTypeId.Decrypt());
            Mapper.Register<CategoryViewModel, CategoryModel>()
                .Member(dest => dest.CategoryId, src => src.CategoryId.Decrypt());
            Mapper.Register<CountryViewModel, CountryModel>()
                .Member(dest => dest.CountryId, src => src.CountryId.Decrypt());
            Mapper.Register<AccountAccountTypeViewModel, AccountAccountTypeModel>()
                   .Member(dest => dest.AccountId, src => src.AccountId.Decrypt())
                       .Member(dest => dest.AccountTypeId, src => src.AccountTypeId.Decrypt())
                 .Member(dest => dest.AccountAccountTypeId, src => src.AccountAccountTypeId.Decrypt());
            Mapper.Register<AccountCategoryViewModel, AccountCategoryModel>()
                                   .Member(dest => dest.AccountId, src => src.AccountId.Decrypt())
                       .Member(dest => dest.AccountCategoryId, src => src.AccountCategoryId.Decrypt())
                 .Member(dest => dest.CategoryId, src => src.CategoryId.Decrypt());
            Mapper.Register<AccountCompanyViewModel, AccountCompanyModel>()
                         .Member(dest => dest.AccountCompanyId, src => src.AccountCompanyId.Decrypt())
                       .Member(dest => dest.AccountId, src => src.AccountId.Decrypt())
                 .Member(dest => dest.CompanyId, src => src.CompanyId.Decrypt());
            Mapper.Register<AttributeCategoryViewModel, AttributeCategoryModel>()
                 .Member(dest => dest.AttributeCategoryId, src => src.AttributeCategoryId.Decrypt())
                 .Member(dest => dest.AttributeId, src => src.AttributeId.Decrypt())
                 .Member(dest => dest.CategoryId, src => src.CategoryId.Decrypt());
            Mapper.Register<AttributeViewModel, AttributeModel>()
                 .Member(dest => dest.AttributeId, src => src.AttributeId.Decrypt())
                 .Member(dest => dest.FeatureTypeId, src => src.FeatureTypeId.Decrypt());
            Mapper.Register<FeatureTypeViewModel, FeatureTypeModel>()
                  .Member(dest => dest.FeatureTypeId, src => src.FeatureTypeId.Decrypt());
            Mapper.Register<StateViewModel, StateModel>()
                  .Member(dest => dest.StateId, src => src.StateId.Decrypt())
                 .Member(dest => dest.CountryId, src => src.CountryId.Decrypt());
#region Product
            Mapper.Register<ProductViewModel, ProductModel>()
                .Member(dest => dest.ProductId, src => src.ProductId.Decrypt())
                .Member(dest => dest.ProductClassId, src => src.ProductClassId.Decrypt());
            Mapper.Register<DivisionViewModel, DivisionModel>()
               .Member(dest => dest.DivisionId, src => src.DivisionId.Decrypt());
            Mapper.Register<AvailabilityViewModel, AvailabilityModel>()
                          .Member(dest => dest.AvailabilityId, src => src.AvailabilityId.Decrypt());
            Mapper.Register<ProductClassViewModel, ProductClassModel>()
                          .Member(dest => dest.ProductClassId, src => src.ProductClassId.Decrypt());

            Mapper.Register<ProductAvailabilityViewModel, ProductAvailabilityModel>()
                          .Member(dest => dest.AvailabilityId, src => src.AvailabilityId.Decrypt())
            .Member(dest => dest.ProductId, src => src.ProductId.Decrypt())
                .Member(dest => dest.ProductAvailabilityId, src => src.ProductAvailabilityId.Decrypt());
            Mapper.Register<ProductAccountPriceViewModel, ProductAccountPriceModel>()
                         .Member(dest => dest.AccountId, src => src.AccountId.Decrypt())
                         .Member(dest => dest.ProductId, src => src.ProductId.Decrypt())
                         .Member(dest => dest.ProductAccountPriceId, src => src.ProductAccountPriceId.Decrypt());
                          
              Mapper.Register<ProductPricingViewModel, ProductPricingModel>()
                          .Member(dest => dest.ProductId, src => src.ProductId.Decrypt())
                          .Member(dest => dest.ProductPricingId, src => src.ProductPricingId.Decrypt());
             Mapper.Register<ProductDivisionViewModel, ProductDivisionModel>()
                          .Member(dest => dest.ProductId, src => src.ProductId.Decrypt())
                          .Member(dest => dest.DivisionId, src => src.DivisionId.Decrypt())
                          .Member(dest => dest.ProductDivisionId, src => src.ProductDivisionId.Decrypt());
             Mapper.Register<ProductCategoryViewModel, ProductCategoryModel>()
                           .Member(dest => dest.ProductId, src => src.ProductId.Decrypt())
                           .Member(dest => dest.CategoryId, src => src.CategoryId.Decrypt())
                           .Member(dest => dest.ProductCategoryId, src => src.ProductCategoryId.Decrypt());
             Mapper.Register<ProductAttributeViewModel, ProductAttributeModel>()
                           .Member(dest => dest.ProductId, src => src.ProductId.Decrypt())
                           .Member(dest => dest.AttributeId, src => src.AttributeId.Decrypt())
                           .Member(dest => dest.ProductAttributeId, src => src.ProductAttributeId.Decrypt());
#endregion




           
            #endregion

       //--------------------------------------------------------------------------------------------
            #region DomainToView

            Mapper.Register<AccountCategoryModel, AccountCategoryViewModel>()
                                  .Member(dest => dest.AccountId, src => src.AccountId.Encrypt())
                      .Member(dest => dest.AccountCategoryId, src => src.AccountCategoryId.Encrypt())
                .Member(dest => dest.CategoryId, src => src.CategoryId.Encrypt());
            Mapper.Register<AccountAccountTypeModel, AccountAccountTypeViewModel>()
                  .Member(dest => dest.AccountId, src => src.AccountId.Encrypt())
                      .Member(dest => dest.AccountTypeId, src => src.AccountTypeId.Encrypt())
                .Member(dest => dest.AccountAccountTypeId, src => src.AccountAccountTypeId.Encrypt());
            Mapper.Register<StateModel, StateViewModel>()
                .Member(dest => dest.StateId, src => src.StateId.Encrypt())
                         .Member(dest => dest.CountryId, src => src.CountryId.Encrypt());
            Mapper.Register<AccountModel, AccountViewModel>()
                    .Member(dest => dest.AccountId, src => src.AccountId.Encrypt())
                            .Member(dest => dest.OwnerId, src => src.OwnerId.Encrypt())
            .Member(dest => dest.CountryId, src => src.CountryId != null ? src.CountryId.Value.Encrypt() : null)
                .Member(dest => dest.StateId, src => src.StateId != null ? src.StateId.Value.Encrypt() : null)
                .Member(dest => dest.CountryType, src => src.CountryModel);
            Mapper.Register<AccountUserModel, AccountUserViewModel>()
                 .Member(dest => dest.AccountId, src => src.AccountId.Encrypt())
                            .Member(dest => dest.UserId, src => src.UserId.Encrypt());
            Mapper.Register<AttributeCategoryModel, AttributeCategoryViewModel>()
               .Member(dest => dest.AttributeCategoryId, src => src.AttributeCategoryId.Encrypt())
               .Member(dest => dest.AttributeId, src => src.AttributeId.Encrypt())
               .Member(dest => dest.CategoryId, src => src.CategoryId.Encrypt());
            Mapper.Register<UserModel, UserViewModel>()
                .Member(dest => dest.AccountId, src => src.AccountId.Encrypt())
            .Member(dest => dest.Id, src => src.Id.Encrypt())
            .Member(dest => dest.CompanyId, src => src.CompanyId != null ? src.CompanyId.Value.Encrypt() : null)
            .Member(dest => dest.CountryId, src => src.CountryId != null ? src.CountryId.Value.Encrypt() : null)
                .Member(dest => dest.StateId, src => src.StateId != null ? src.StateId.Value.Encrypt() : null)
             .Member(dest => dest.CountryType, src => src.CountryModel);
            Mapper.Register<CategoryModel, CategoryViewModel>()
                    .Member(dest => dest.CategoryId, src => src.CategoryId.Encrypt());
            Mapper.Register<AccountTypeModel, AccountTypeViewModel>()
                 .Member(dest => dest.AccountTypeId, src => src.AccountTypeId.Encrypt());
            Mapper.Register<CountryModel, CountryViewModel>()
              .Member(dest => dest.CountryId, src => src.CountryId.Encrypt());

            Mapper.Register<AttributeModel, AttributeViewModel>()
                .Member(dest => dest.AttributeId, src => src.AttributeId.Encrypt())
                .Member(dest => dest.FeatureTypeName, src => src.FeatureType.Name)
                .Member(dest => dest.FeatureTypeId, src => src.FeatureTypeId.Encrypt());

            Mapper.Register<FeatureTypeModel, FeatureTypeViewModel>()
            .Member(dest => dest.FeatureTypeId, src => src.FeatureTypeId.Encrypt());
           
            #region Product
            Mapper.Register<ProductModel, ProductViewModel>()
                .Member(dest => dest.ProductClassName, src => src.ProductClassModel!= null ? src.ProductClassModel.Name : null)
                .Member(dest => dest.ProductId, src => src.ProductId != null ? src.ProductId.Encrypt() : null)
                .Member(dest => dest.ProductClassId, src => src.ProductClassId != null ? src.ProductClassId.Value.Encrypt() : null);
            Mapper.Register<DivisionModel,DivisionViewModel>()
               .Member(dest => dest.DivisionId, src => src.DivisionId.Encrypt());

            Mapper.Register<AvailabilityModel,AvailabilityViewModel>()
                .Member(dest => dest.AvailabilityId, src => src.AvailabilityId!=null? src.AvailabilityId.Encrypt():null);

            Mapper.Register<ProductClassModel, ProductClassViewModel>()
                .Member(dest => dest.ProductClassId, src =>src.ProductClassId!=null? src.ProductClassId.Encrypt():null);

            Mapper.Register<ProductAvailabilityModel, ProductAvailabilityViewModel>()
                .Member(dest => dest.AvailabilityId, src => src.AvailabilityId != null ? src.AvailabilityId.Encrypt() : null)
            .Member(dest => dest.ProductId, src => src.ProductId != null ? src.ProductId.Encrypt() : null)
                .Member(dest => dest.ProductAvailabilityId, src => src.ProductAvailabilityId != null ? src.ProductAvailabilityId.Encrypt() : null);
                
            Mapper.Register<ProductAccountPriceModel,ProductAccountPriceViewModel >()
                          .Member(dest => dest.AccountId, src => src.AccountId.Encrypt())
                         .Member(dest => dest.ProductId, src => src.ProductId.Encrypt())
                         .Member(dest => dest.ProductAccountPriceId, src => src.ProductAccountPriceId.Encrypt());
              Mapper.Register<ProductPricingModel,ProductPricingViewModel>()
                          .Member(dest => dest.ProductId, src => src.ProductId.Encrypt())
                          .Member(dest => dest.ProductPricingId, src => src.ProductPricingId.Encrypt());
             Mapper.Register<ProductDivisionModel,ProductDivisionViewModel>()
                          .Member(dest => dest.ProductId, src => src.ProductId.Encrypt())
                          .Member(dest => dest.DivisionId, src => src.DivisionId.Encrypt())
                          .Member(dest => dest.ProductDivisionId, src => src.ProductDivisionId.Encrypt());
             Mapper.Register<ProductCategoryModel,ProductCategoryViewModel>()
                         .Member(dest => dest.ProductId, src => src.ProductId.Encrypt())
                         .Member(dest => dest.CategoryId, src => src.CategoryId.Encrypt())
                         .Member(dest => dest.ProductCategoryId, src => src.ProductCategoryId.Encrypt());
             Mapper.Register<ProductAttributeModel,ProductAttributeViewModel>()
                            .Member(dest => dest.ProductId, src => src.ProductId.Encrypt())
                            .Member(dest => dest.AttributeId, src => src.AttributeId.Encrypt())
                            .Member(dest => dest.ProductAttributeId, src => src.ProductAttributeId.Encrypt());

            #endregion

            //.Member(dest => dest.FullName, src => src.FirstName)
            //.Member(dest => dest.Email, src => src.UserName)
            //.Member(dest=>dest.CompanyName,src=>src.CompanyModel.Name);

             Mapper.Register<CategoryModel, TreeViewControlModel>()
        .Member(dest => dest.Id, src => src.CategoryId.Encrypt())
        .Member(dest => dest.ParentId, src => src.ParentId != null ? src.ParentId.Value.Encrypt() : null)
          .Member(dest => dest.name, src => src.Name);
            Mapper.Register<AutoCompleteModel, AutoCompleteViewModel>()
                 .Member(dest => dest.Id, src => src.Id.Encrypt());
            #endregion
        }
    }
}