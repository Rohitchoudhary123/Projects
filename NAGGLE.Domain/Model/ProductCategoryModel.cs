using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class ProductCategoryModel
    {
        public int ProductCategoryId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
