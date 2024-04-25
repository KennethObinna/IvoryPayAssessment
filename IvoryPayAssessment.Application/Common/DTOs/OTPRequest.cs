using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class OTPRequest
    {
        [JsonIgnore]
        public string Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public OTPType OTPType { get; set; }

    }
 

    public class TwoFAOTPValidateRequest
    {

        public string Email { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }

    }
    public class ValidateOTPRequest
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public OTPType OTPType { get; set; }

    }
}
