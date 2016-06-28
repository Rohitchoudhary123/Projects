using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class ContactModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public CountryModel CountryModel { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }
        public bool IsDeleted { get; set; }
        public int MobileNumber { get; set; }
        public int PhoneNumber { get; set; }
        public int FaxNumber { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
    }
}
