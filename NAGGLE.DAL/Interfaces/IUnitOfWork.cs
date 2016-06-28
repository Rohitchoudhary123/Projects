using NAGGLE.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.DAL.Interfaces
{
    public interface IUnitofWork : IDisposable
    {
        NaggleEntities GetContext();
        void Save();

        IRepository<Country> countryRepository { get; }        
        IRepository<State> stateRepository { get; }        
        IRepository<Company> companyRepository{get;}
        IRepository<Account> accountRepository { get; }
        IRepository<User> userRespository { get; }
        IRepository<AccountType> accountTypeRespository{ get; }        
        IRepository<Category> categoryRespository{ get; }
        IRepository<NAGGLE.DAL.Entities.Attribute> attributeRespository { get; }
        IRepository<FeatureType> featureTypeRespository { get; }


        IRepository<Product> productRespository { get; }
        IRepository<ProductClass> productClassRespository { get; }
        IRepository<Availability> availabilityRespository{get;}
        IRepository<Division> divisionRespository { get; }
        
        
        #region Mapping Table Repository
        IRepository<AccountAccountType> accountAccountTypeRespository{ get; }
        IRepository<AccountCategory> accountCategoryRespository { get; }
        IRepository<AccountCompany> accountCompanyRespository { get; }
        IRepository<AccountContact> accountContactRespository { get; }
        IRepository<AttributeCategory> attributeCategoryRespository { get; }

        #region Product
        IRepository<ProductAvailability> productAvailabilityRespository{ get; }
         IRepository<ProductAccountPrice> productAccountPriceRespository{ get; }
         IRepository<ProductPricing> productPricingRespository{ get; }
         IRepository<ProductDivision> productDivisionRespository { get; }
         IRepository<ProductCategory> productCategoryRespository{ get; }
         IRepository<ProductAttribute> productAttributeRespository { get; }
        
             #endregion



        #endregion

    }
}
