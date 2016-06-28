using NAGGLE.DAL.Entities;
using NAGGLE.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.DAL.Repository
{
    public class UnitOfWork : IUnitofWork
    {
        private NaggleEntities _context;
        private bool _isDisposed;

        public UnitOfWork(NaggleEntities context)
        {
            this._context = context;
        }
        public NaggleEntities GetContext()
        {
            return _context;
        }
        public void Dispose()
        {
            if (_context != null && !_isDisposed)
            {
                _context.Dispose();
                _isDisposed = true;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IRepository<Country> countryRepository
        {
            get { return new BaseRepository<Country>(GetContext()); }
        }
        public IRepository<State> stateRepository
        {
            get { return new BaseRepository<State>(GetContext()); }
        }
        
        public IRepository<Company> companyRepository
        {
            get { return new BaseRepository<Company>(GetContext()); }
        }
        
        public IRepository<Account> accountRepository
        {
            get { return new BaseRepository<Account>(GetContext()); }
        }

        public IRepository<User> userRespository
        {
            get { return new BaseRepository<User>(GetContext()); }
          
        }
        public IRepository<AccountType> accountTypeRespository
        {
            get { return new BaseRepository<AccountType>(GetContext()); }

        }
        public IRepository<Category> categoryRespository
        {
            get { return new BaseRepository<Category>(GetContext()); }

        }

        /*Attribute Repository*/

        public IRepository<NAGGLE.DAL.Entities.Attribute> attributeRespository
        {
            get { return new BaseRepository<NAGGLE.DAL.Entities.Attribute>(GetContext()); }

        }
    
        public IRepository<FeatureType> featureTypeRespository
        {
            get { return new BaseRepository<FeatureType>(GetContext()); }

        }

        /*Product Repository*/
        #region Product
        public IRepository<Product> productRespository
        {
            get { return new BaseRepository<Product>(GetContext()); }

        }
        public IRepository<Availability> availabilityRespository
        {
            get { return new BaseRepository<Availability>(GetContext()); }

        }
        public IRepository<Division> divisionRespository
        {
            get { return new BaseRepository<Division>(GetContext()); }

        }
        public IRepository<ProductClass> productClassRespository
        {
            get { return new BaseRepository<ProductClass>(GetContext()); }

        }

        
      
        
        #endregion 


        #region Mapping Table Repository

        public IRepository<AccountAccountType> accountAccountTypeRespository
        {
            get { return new BaseRepository<AccountAccountType>(GetContext()); }

        }
        public IRepository<AccountCategory> accountCategoryRespository
        {
            get { return new BaseRepository<AccountCategory>(GetContext()); }

        }
        public IRepository<AccountCompany> accountCompanyRespository
        {
            get { return new BaseRepository<AccountCompany>(GetContext()); }

        }
        public IRepository<AccountContact> accountContactRespository
        {
            get { return new BaseRepository<AccountContact>(GetContext()); }

        }
        public IRepository<AttributeCategory> attributeCategoryRespository
        {
            get { return new BaseRepository<AttributeCategory>(GetContext()); }

        }

        #region Product
        public IRepository<ProductAvailability> productAvailabilityRespository
        {
            get { return new BaseRepository<ProductAvailability>(GetContext()); }

        }
        public IRepository<ProductAccountPrice> productAccountPriceRespository
        {
            get { return new BaseRepository<ProductAccountPrice>(GetContext()); }

        }
        public IRepository<ProductPricing> productPricingRespository
        {
            get { return new BaseRepository<ProductPricing>(GetContext()); }

        }

        public IRepository<ProductDivision> productDivisionRespository
        {
            get { return new BaseRepository<ProductDivision>(GetContext()); }

        }
        public IRepository<ProductCategory> productCategoryRespository
        {
            get { return new BaseRepository<ProductCategory>(GetContext()); }

        }
        public IRepository<ProductAttribute> productAttributeRespository
        {
            get { return new BaseRepository<ProductAttribute>(GetContext()); }

        }


        #endregion
        #endregion

    }
}
