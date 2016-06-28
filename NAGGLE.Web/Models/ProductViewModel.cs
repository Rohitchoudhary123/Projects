using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            
            this.ProductAccountPrices = new List<ProductAccountPriceViewModel>();
            this.ProductAttributes = new List<ProductAttributeViewModel>();
            this.ProductAvailabilities = new List<ProductAvailabilityViewModel>();
            this.ProductCategories = new List<ProductCategoryViewModel>();
            this.ProductDivisions = new List<ProductDivisionViewModel>();
            this.ProductPrices = new List<ProductPricingViewModel>();
        }

        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductTag { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int Unit { get; set; }
        public string Description { get; set; }
        public string ProductClassId { get; set; }
        public string ProductClassName{ get; set; }
        public int StockType { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDefined { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public bool Status { get; set; }


        public List<ProductAccountPriceViewModel> ProductAccountPrices { get; set; }
        public List<ProductAttributeViewModel> ProductAttributes { get; set; }
        public List<ProductAvailabilityViewModel> ProductAvailabilities { get; set; }
        public List<ProductCategoryViewModel> ProductCategories { get; set; }
        public List<ProductDivisionViewModel> ProductDivisions { get; set; }
        public List<ProductPricingViewModel> ProductPrices { get; set; }
    }
}