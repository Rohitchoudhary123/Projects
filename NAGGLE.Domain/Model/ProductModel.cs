using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class ProductModel
    {
        public ProductModel()
        {
            this.ProductClassModel = new ProductClassModel();
            this.ProductAccountPriceModel = new List<ProductAccountPriceModel>();
            this.ProductAttributeModel = new List<ProductAttributeModel>();
            this.ProductAvailabilityModel = new List<ProductAvailabilityModel>();
            this.ProductCategoryModel = new List<ProductCategoryModel>();
            this.ProductDivisionModel = new List<ProductDivisionModel>();
            this.ProductPricingModel = new List<ProductPricingModel>();
        }
    
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductTag { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Unit { get; set; }
        public string Description { get; set; }
        public Nullable<int> ProductClassId { get; set; }
        public Nullable<int> StockType { get; set; }
        public Nullable<bool> IsPublic { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> IsDefined { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    
        public  ProductClassModel ProductClassModel { get; set; }
        public  List<ProductAccountPriceModel> ProductAccountPriceModel{ get; set; }
        public List<ProductAttributeModel> ProductAttributeModel { get; set; }
        public  List<ProductAvailabilityModel> ProductAvailabilityModel{ get; set; }
        public List<ProductCategoryModel> ProductCategoryModel { get; set; }
        public  List<ProductDivisionModel> ProductDivisionModel { get; set; }
        public  List<ProductPricingModel> ProductPricingModel { get; set; }
    }
}
