using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class ProductDivisionModel
    {
        public int ProductDivisionId { get; set; }
        public int ProductId { get; set; }
        public int DivisionId { get; set; }
        public string Name { get; set; }
        
    }
}
