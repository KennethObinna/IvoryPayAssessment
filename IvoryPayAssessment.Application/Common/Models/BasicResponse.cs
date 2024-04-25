

using System.ComponentModel;

namespace IvoryPayAssessment.Application.Common.Models
{
    public class BasicResponse
    {
        [DefaultValue(true)]
        public bool IsSuccessful { get; set; }
        [DefaultValue(null)]
        public ErrorResponse Error { get; set; }

       
        public BasicResponse(bool isSuccessful=false)
        {
            IsSuccessful = isSuccessful;
        }
      


    }
}
