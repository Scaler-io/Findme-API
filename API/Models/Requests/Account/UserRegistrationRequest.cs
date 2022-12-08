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
        public UserProfileRequest Profile { get; set; }
        public UserAddressRequest Address { get; set; }
    }

    public class UserAddressRequest
    {
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
    }

    public class UserProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
    }
}
