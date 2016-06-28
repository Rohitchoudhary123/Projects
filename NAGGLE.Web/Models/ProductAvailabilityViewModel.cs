using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class ProductAvailabilityViewModel
    {
        public string ProductAvailabilityId { get; set; }
        public string ProductId { get; set; }
        public string AvailabilityId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public AvailabilityViewModel Availability { get; set; }
    }
}