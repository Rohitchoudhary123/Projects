using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class ProductAccountPriceModel
    {
        public AccountModel AccountModel { get; set; }
        public int ProductAccountPriceId { get; set; }
        public int ProductId { get; set; }
        public int AccountId { get; set; }
        public int Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
