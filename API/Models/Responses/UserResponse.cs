using API.Models.Core;

namespace API.Models.Responses
{
    public class UserResponse
    {
        public string Username { get; set; }
        public UserMetadata Metadata { get; set; }
    }
}
