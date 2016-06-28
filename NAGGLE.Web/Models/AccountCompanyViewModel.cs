using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class AccountCompanyViewModel
    {
        public AccountCompanyViewModel()
       {
           this.Account = new AccountViewModel();
           this.Company = new CompanyViewModel();
       }
        public string AccountCompanyId { get; set; }
        public string AccountId { get; set; }
        public string CompanyId { get; set; }

        public AccountViewModel Account { get; set; }
        public CompanyViewModel Company { get; set; }
    }
}