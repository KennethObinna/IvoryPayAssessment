 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class OTPs: BaseObject
    {
        public OTPs()
        {
            //DateCreated= DateTime.Now;
        }
     
        public string OTP { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string OTPType { get; set; }
        public string? Token { get; set; }

        //public string MerchantId { get; set; }


    }
}
