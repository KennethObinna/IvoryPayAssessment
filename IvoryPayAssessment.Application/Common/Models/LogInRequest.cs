namespace IvoryPayAssessment.Application.Common.Models
{  public class LogInRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
       // public bool IsTwoFAEnabled { get; set; }
        //public TwoFAOTPRequest TwoFAOTPRequest { get; set; }
    }
    public class LogInRequest2
    {
        public string Email { get; set; }
        public string Password { get; set; }   
        public string Code { get; set; }   
        public bool IsTwoFAEnabled { get; set; }
        
    }
}
