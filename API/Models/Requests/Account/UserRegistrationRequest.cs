using Destructurama.Attributed;

namespace API.Models.Requests.Account
{
    public class UserRegistrationRequest
    {
        public string Username { get; set; }
        
        [LogMasked]
        public string Password { get; set; }

        [LogMasked]
        public string ConfirmPassword { get; set; }
    }
}
