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
    
    public partial class User
    {
        public User()
        {
            this.AccountContact = new HashSet<AccountContact>();
            this.Company = new HashSet<Company>();
            this.Account = new HashSet<Account>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> Shipments { get; set; }
        public int ShipmentType { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string BillAddressLine1 { get; set; }
        public string BillAddressLine2 { get; set; }
        public string BillAddressLine3 { get; set; }
        public string BillCity { get; set; }
        public Nullable<int> BillStateId { get; set; }
        public string BillPostalCode { get; set; }
        public string BillTelephone { get; set; }
        public string BillEmail { get; set; }
        public Nullable<int> BillCountryId { get; set; }
        public string BillCompany { get; set; }
        public string BillContactPerson { get; set; }
        public string FaxNumber { get; set; }
        public string Designation { get; set; }
    
        public virtual ICollection<AccountContact> AccountContact { get; set; }
        public virtual ICollection<Company> Company { get; set; }
        public virtual Company Company1 { get; set; }
        public virtual Country Country { get; set; }
        public virtual Country Country1 { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual User User11 { get; set; }
        public virtual User User3 { get; set; }
        public virtual ICollection<Account> Account { get; set; }
        public virtual State State { get; set; }
    }
}
