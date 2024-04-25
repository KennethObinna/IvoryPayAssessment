 
 

namespace IvoryPayAssessment.Application.Common.Models
{
    public class BaseRequestModel
    {
        [JsonIgnore] 
        public string Language { get; set; }
    }
}
