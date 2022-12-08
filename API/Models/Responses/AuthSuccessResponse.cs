using Destructurama.Attributed;

namespace API.Models.Responses
{
    public class AuthSuccessResponse
    {
        public UserResponse User { get; set; }

        [LogMasked(ShowFirst = 5)]
        public string Token { get; set; }
    }
}
