using API.Models.Constants;

namespace API.Models.Core
{
    public class ApiResponse
    {
        public ApiResponse(string code, string message = "")
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }
        public string Message { get; set; }

        protected virtual string GetDefaultMessage(string statusCode){
            return statusCode switch{
                ErrorCodes.BadRequest          => ErrorMessages.BadRequest,
                ErrorCodes.Unauthorized        => ErrorMessages.Unauthorized,
                ErrorCodes.NotFound            => ErrorMessages.NotFound,
                ErrorCodes.Operationfailed     => ErrorMessages.Operationfailed,
                ErrorCodes.InternalServerError => ErrorMessages.InternalServerError,
                _                              => string.Empty
            };
        }
    }
}