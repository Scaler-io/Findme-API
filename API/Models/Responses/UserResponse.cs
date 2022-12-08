using API.Models.Core;

namespace API.Models.Responses
{
    public class UserResponse
    {
        public string Username { get; set; }
        public UserProfileResponse Profile { get; set; }
        public UserMetadata Metadata { get; set; }
        public UserAddressResponse Address { get; set; }
        public ICollection<UserImageResponse> Photos { get; set; } = new List<UserImageResponse>();
    }
}
