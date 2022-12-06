using API.Models.Core;

namespace API.Models.Responses
{
    public class AuthSuccessResponse
    {
        public string Username { get; set; }
        public UserMetadata Metadata { get; set; }
    }
}
