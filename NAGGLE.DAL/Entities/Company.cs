//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NAGGLE.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Company
    {
        public Company()
        {
            this.AccountCompany = new HashSet<AccountCompany>();
            this.Role = new HashSet<Role>();
            this.User1 = new HashSet<User>();
        }
    
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    
        public virtual ICollection<AccountCompany> AccountCompany { get; set; }
        public virtual ICollection<Role> Role { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<User> User1 { get; set; }
    }
}
