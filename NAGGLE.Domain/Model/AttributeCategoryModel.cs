using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class AttributeCategoryModel
    {
        public int AttributeCategoryId { get; set; }
        public int AttributeId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
