﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class AccountTypeViewModel
    {

         public AccountTypeViewModel()
        {
          //  this.AccountAccountType = new HashSet<AccountAccountType>();
        }
    
        public string AccountTypeId { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    
       // public virtual ICollection<AccountAccountType> AccountAccountType { get; set; }
    }

    }
