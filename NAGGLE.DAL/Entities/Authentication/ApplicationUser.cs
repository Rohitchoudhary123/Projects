using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NAGGLE.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.DAL.Entities
{
    public partial class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
        , IUser<int>, IFlagRemove, ILogInfo
    {
        //public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public Nullable<DateTime> ActivatedOn { get; set; }
        public string BusinessName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> Shipments { get; set; }
        public int ShipmentType { get; set; }

        [MaxLength(500)]
        public string AddressLine1 { get; set; }
        [MaxLength(500)]
        public string AddressLine2 { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        public int? StateId { get; set; }
        [MaxLength(15)]
        public string PostalCode { get; set; }
        [MaxLength(20)]
        public string Telephone { get; set; }
        public int? CountryId { get; set; }

        [MaxLength(50)]
        public string BillCompany { get; set; }
        [MaxLength(50)]
        public string BillContactPerson { get; set; }

        [MaxLength(500)]
        public string BillAddressLine1 { get; set; }
        [MaxLength(500)]
        public string BillAddressLine2 { get; set; }
        [MaxLength(500)]
        public string BillAddressLine3 { get; set; }
        [MaxLength(50)]
        public string BillCity { get; set; }
        public int? BillStateId { get; set; }
        [MaxLength(15)]
        public string BillPostalCode { get; set; }
        [MaxLength(20)]
        public string BillTelephone { get; set; }
        [MaxLength(50)]
        public string BillEmail { get; set; }
        public int? BillCountryId { get; set; }
        public string Designation { get; set; }
        public string FaxNumber { get; set; }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, bool isThirdParty = false)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim("CompanyId", this.CompanyId.ToString()));
            userIdentity.AddClaim(new Claim("FullName", this.FirstName + " " + this.LastName));
            //if (AccountUsers.Count > 0)
            //{
            //    userIdentity.AddClaim(new Claim(ClaimType.SelectedAccount.ToString(), Convert.ToString(this.AccountUsers.FirstOrDefault().AccountID)));
            //    userIdentity.AddClaim(new Claim(ClaimType.AccountIDs.ToString(), Convert.ToString(this.AccountUsers.Select(e => e.AccountID).ToArray())));
            //    userIdentity.AddClaim(new Claim(ClaimType.AccountNames.ToString(), Convert.ToString(this.AccountUsers.Select(e => e.Account.AccountName).ToArray())));
            //    userIdentity.AddClaim(new Claim(ClaimType.IsSuperAdmin.ToString(), "false"));
            //}
            //else
            //{
            //    userIdentity.AddClaim(new Claim("IsSuperAdmin", "true"));
            //}
            // Add custom user claims here 
            return Task.FromResult(userIdentity);
        }

    }
}
