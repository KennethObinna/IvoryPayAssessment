

namespace IvoryPayAssessment.Application.Common.Models
{
    public class ErrorResponse
    {
        public string responseCode { get; set; }
        public string responseDescription { get; set; }
      
       


        public static T Create<T>(string errorCode, string errorMessage) where T : BasicResponse, new()
        {
            var response = new T
            {
                IsSuccessful = false,
                Error = new ErrorResponse
                {
                    responseCode = errorCode,
                    responseDescription = errorMessage
                }
            };
            return response;
        }

        public override string ToString()
        {
            return $"{responseCode} :-: {responseDescription}";
        }

    }
}
