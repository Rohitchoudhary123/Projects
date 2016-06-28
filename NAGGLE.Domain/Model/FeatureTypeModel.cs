using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
  public  class FeatureTypeModel
    {
       public FeatureTypeModel()
        {
            //this.Attribute = new HashSet<Attribute>();
        }
    
        public int FeatureTypeId { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        //public virtual ICollection<Attribute> Attribute { get; set; }
    }
}
