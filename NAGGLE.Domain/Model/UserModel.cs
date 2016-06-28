using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Domain.Model
{
    public class UserModel
    {
        public UserModel()
        {
            this.CountryModel = new CountryModel();
            this.CompanyModel = new CompanyModel();
           
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ShipmentType { get; set; }
        public string BusinessName { get; set; }     
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? CompanyId { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber{ get; set; }
        public string Telephone { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Designation { get; set; }
        public List<AccountModel>  AccountModel { get; set; }
        public CompanyModel CompanyModel { get; set; }
        public CountryModel CountryModel { get; set; }

    }
}
