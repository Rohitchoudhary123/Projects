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
    
    public partial class AccountAccountType
    {
        public int AccountAccountTypeId { get; set; }
        public int AccountTypeId { get; set; }
        public int AccountId { get; set; }
    
        public virtual AccountType AccountType { get; set; }
        public virtual Account Account { get; set; }
    }
}