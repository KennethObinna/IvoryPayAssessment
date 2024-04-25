using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Interfacses.OTPServices
{
    public interface IOTPService
    {
        Task<ServerResponse<string>> GenerateOTP(OTPRequest request,bool isVerifyemail = true, bool resend=false);
      Task<ServerResponse<bool>> ValidateOTP(ValidateOTPRequest request,bool isInternal=true);
    }
}
