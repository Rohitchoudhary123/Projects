using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class ProductAvailabilityModel
    {
        public int ProductAvailabilityId { get; set; }
        public int ProductId { get; set; }
        public int AvailabilityId { get; set; }
        public bool IsDeleted { get; set; }
        public AvailabilityModel AvailabilityModel { get; set; }
    }
}
