﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
   public class CompanyModel
    {

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

      
    }
}
