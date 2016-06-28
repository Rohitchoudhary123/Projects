using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NAGGLE.Web.Models
{
    public class UserViewModel
    {
        /*Use on SignUp  Page*/
        public string Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }
        public string CompanyName { get; set; }      
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        
        /*Use on Account Page  */
        public string AccountId { get; set; }
        public string FaxNumber { get; set; }
        public string Telephone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string CompanyId { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string WebSite { get; set; }
        public string Description { get; set; }
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string AddressLine1 { get; set; }
        public string Designation { get; set; }
       
        public CountryViewModel CountryType { get; set; }

        
    }
}