using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAGGLE.Web.Models
{
    public class ProductAttributeViewModel
    {
        public string ProductAttributeId { get; set; }
        public string ProductId { get; set; }
        public string AttributeId { get; set; }
        public bool IsDeleted { get; set; }
    }
}