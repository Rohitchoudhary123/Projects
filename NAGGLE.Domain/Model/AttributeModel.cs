using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class AttributeModel
    {
        public AttributeModel()
        {
            this.AttributeCategoryModel = new List<AttributeCategoryModel>();
        }
    
        public int AttributeId { get; set; }
        public string FeatureName { get; set; }
        public int FeatureTypeId { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public bool Status { get; set; }
        public  FeatureTypeModel FeatureType { get; set; }
        public List<AttributeCategoryModel> AttributeCategoryModel { get; set; }
    }
}
