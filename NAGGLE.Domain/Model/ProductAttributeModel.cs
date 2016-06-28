using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class ProductAttributeModel
    {
        public int ProductAttributeId { get; set; }
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
