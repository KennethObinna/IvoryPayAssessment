using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.Enums
{
    public enum OTPType
    {
        ChangePassword=1, 
        ResetPassword=2, 
        VerifyEmail=3, 
        ClientId=4, 
        ClientKey=5, 
        VerifyPhoneNumber=6,
        UpdateUnverifiedEmail = 7,
        UpdateUnverifiedPhone = 8,
        TwoFactorAuthentication=9
    }
    public enum RequestType
    {
        PrimaryEmailVerification = 1, PrimaryEmailUpdate = 2, PrimaryPhoneNumberVerification = 3, PrimaryPhoneNumberUpdate = 4,
        SecondaryPhoneNumberVerification = 5, SecondaryPhoneNumberUpdate = 6, SecondaryEmailVerification = 7,
        TwoFactorAuthentication=8
    }
    public enum RequestStatus
    {
        Pending = 1, Approved = 2, Rejected = 3
    }
}
