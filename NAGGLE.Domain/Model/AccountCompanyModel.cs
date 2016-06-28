using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
   public class AccountCompanyModel
    {
       public AccountCompanyModel()
       {
           this.AccountModel = new AccountModel();
           this.CompanyModel = new CompanyModel();
       }
        public int AccountCompanyId { get; set; }
        public int AccountId { get; set; }
        public int CompanyId { get; set; }

        public  AccountModel AccountModel { get; set; }
        public CompanyModel CompanyModel { get; set; }

    }
}
