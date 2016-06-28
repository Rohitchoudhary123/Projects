using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class AccountUserViewModel
    {
        public string AccountId { get; set; }
        public string UserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        public UserViewModel UserViewModel { get; set; }
        public AccountViewModel AccountViewModel { get; set; }
    }
}