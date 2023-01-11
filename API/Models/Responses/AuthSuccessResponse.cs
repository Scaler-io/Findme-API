using Destructurama.Attributed;

namespace API.Models.Responses
{
    public class AuthSuccessResponse
    {
        public string Username { get; set; }
        public string Address { get; set; }
        public List<string> Roles { get; set; }
        [LogMasked(ShowFirst = 5)]
        public string Token { get; set; }
    }
}
