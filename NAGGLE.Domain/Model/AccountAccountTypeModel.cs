using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class AccountAccountTypeModel
    {
        public int AccountAccountTypeId { get; set; }
        public int AccountTypeId { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
    }
}
