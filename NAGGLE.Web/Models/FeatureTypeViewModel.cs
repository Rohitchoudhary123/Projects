using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class FeatureTypeViewModel
    {
        public FeatureTypeViewModel()
        {
            //this.Attribute = new HashSet<Attribute>();
        }
    
        public string FeatureTypeId { get; set; }
        public string Name { get; set; }
		public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        //public virtual ICollection<Attribute> Attribute { get; set; }
    }
}