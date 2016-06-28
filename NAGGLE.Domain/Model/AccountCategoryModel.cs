using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class AccountCategoryModel
    {
        public int AccountCategoryId { get; set; }
        public int CategoryId { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
    }
}
