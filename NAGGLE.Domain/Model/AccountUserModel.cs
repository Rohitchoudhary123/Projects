using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class AccountUserModel
    {
        public AccountUserModel()
        {
       //     this.AccountModel = new AccountModel();
        //this.UserModel = new UserModel();
        }

        public int AccountId { get; set; }
        public int UserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        public  UserModel UserModel { get; set; }
        public AccountModel AccountModel { get; set; }
    }
}
