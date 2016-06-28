using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class AttributeViewModel
    {
        public AttributeViewModel()
        {
             this.AttributeCategories = new List<AttributeCategoryViewModel>();
        }

        public string AttributeId { get; set; }
        public string FeatureName { get; set; }
        public string FeatureTypeId { get; set; }
        public string FeatureTypeName { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public bool Status { get; set; }
        // public string [checked]  {get;set;;}
        //public  FeatureType FeatureType { get; set; }
        public List<AttributeCategoryViewModel> AttributeCategories{ get; set; }
    }   
}