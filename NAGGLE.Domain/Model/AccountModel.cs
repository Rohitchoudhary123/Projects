using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
  public  class AccountModel
    {
      public AccountModel()
      {
          this.UserModel = new UserModel();
          this.AccountUserModel = new List<AccountUserModel>();
          this.AccountAccountTypeModel = new List<AccountAccountTypeModel>();
          this.AccountCategoryModel = new List<AccountCategoryModel >();
          this.AccountCompanyModel = new List<AccountCompanyModel>();
          this.CountryModel = new CountryModel();
      }

        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public Nullable<int> CountryId { get; set; }
        public CountryModel CountryModel { get; set; }
        public Nullable<int> StateId { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int OwnerId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }
        public bool IsDeleted { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public UserModel UserModel { get; set; }
        //public  List<Accountcon> AccountAccountType { get; set; }
        public List<AccountAccountTypeModel> AccountAccountTypeModel { get; set; }
        public List<AccountCategoryModel> AccountCategoryModel { get; set; }
        public List<AccountUserModel> AccountUserModel { get; set; }
        public List<AccountCompanyModel> AccountCompanyModel { get; set; }

        

    }
}
