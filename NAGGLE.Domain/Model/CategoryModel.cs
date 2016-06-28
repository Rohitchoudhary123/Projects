using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
   public class CategoryModel
    {

        public CategoryModel()
        {
            this.AccountCategoryModel = new HashSet<AccountCategoryModel>();
        }
    
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool IsSelectable { get; set; }
        public bool isParent { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    
        public virtual ICollection<AccountCategoryModel> AccountCategoryModel { get; set; }
    }

    
}
