using IvoryPayAssessment.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public  class UserDto
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }    
        public string Email { get; set; }    
        public string PhoneNumber { get; set; }    
        public string UserId { get; set; }      
  
        public string AccountMode { get; set; }
     
 
        public DateTime DateCreated { get; set; }
        public DateTime DateofBirth { get; set; }       
 
     
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
      
        public string DefaultRole { get; set; }
        public string HomeAddress { get; set; }
        public string Address { get; set; }  
        public string ImageUrl { get; set; }            
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
       
    }

    public partial class UserDtoV2
    {
        public string Name { get; set; }

        public string Id { get; set; }


    }
}
