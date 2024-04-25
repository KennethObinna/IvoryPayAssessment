using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.Models
{
    public class UserLoginResponseModel
    {
        //public string UserName { get; set; }
        //public string Email { get; set; }
       public string FullName { get; set; }
        //public string BusinessName { get; set; }
        //public string ClientId { get; set; }
        public bool isEmailConfirmed { get; set; }
        public Tokens Token { get; set; }
        public string? UserType { get; set; }
        public string? ReturnUrl { get; set; }
        public RoleModelView RoleModelView { get; set; }
        public UserDto User { get; set; }
      
       
    }
    
}
