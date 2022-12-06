using API.Models.Core;
using Destructurama.Attributed;

namespace API.Models.Responses
{
    public class AuthSuccessResponse
    {
        public string Username { get; set; }
        public UserMetadata Metadata { get; set; }
        
        [LogMasked(ShowFirst = 5)]
        public string Token { get; set; }
    }
}
