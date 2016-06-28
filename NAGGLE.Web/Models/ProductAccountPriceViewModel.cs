using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class ProductAccountPriceViewModel
    {
        public AccountViewModel Account{ get; set; }
        public string ProductAccountPriceId { get; set; }
        public string ProductId { get; set; }
        public string AccountId { get; set; }
        public int Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}