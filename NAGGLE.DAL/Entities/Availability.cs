//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NAGGLE.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Availability
    {
        public Availability()
        {
            this.ProductAvailability = new HashSet<ProductAvailability>();
        }
    
        public int AvailabilityId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<ProductAvailability> ProductAvailability { get; set; }
    }
}
