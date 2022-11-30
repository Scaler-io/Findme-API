using API.Models.Constants;

namespace API.Models.Core
{
    public class ApiExceptionResponse: ApiResponse
    {
        public string StackTrace { get; set; }
        public ApiExceptionResponse(string errorMessage = "", string stackTrace = "")
            :base(ErrorCodes.InternalServerError, errorMessage)
        {
            StackTrace = stackTrace;
        }
    }
}