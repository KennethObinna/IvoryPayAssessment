using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class Tokens
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }


    public class TokenConvert
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public UserDto User { get; set; }
        public string UserId { get; set; }


    }
}
