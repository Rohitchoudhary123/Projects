using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class AccountViewModel
    {
        public AccountViewModel()
      {
          this.User = new UserViewModel();
          this.AccountUsers = new List<AccountUserViewModel>();
          this.AccountAccountTypes = new List<AccountAccountTypeViewModel>();
          this.AccountCategories = new List<AccountCategoryViewModel>();
          this.AccountCompanies= new List<AccountCompanyViewModel>();
          this.CountryType = new CountryViewModel();
      }
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string CountryId { get; set; }
        public CountryViewModel CountryType { get; set; }
        public string StateId { get; set; }
        public StateViewModel State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }
        public string OwnerId { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public UserViewModel User { get; set; }

        public List<AccountAccountTypeViewModel> AccountAccountTypes { get; set; }
        public List<AccountCategoryViewModel> AccountCategories{ get; set; }
        public List<AccountCompanyViewModel> AccountCompanies { get; set; }

        //public virtual ICollection<AccountAccountType> AccountAccountType { get; set; }

        public List<AccountUserViewModel> AccountUsers { get; set; }
    }
}