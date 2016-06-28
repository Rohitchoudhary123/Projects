using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Common.Utility
{
  public  class FilterParameter
    {
        public string AccountTypeId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public string SearchText { get; set; }
        public Nullable<bool> IsPublic { get; set; }//It's use in Product Module
    }
}
