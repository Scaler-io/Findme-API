using API.Entities;

namespace API.Models.Requests.User
{
    public class UserUpdateRequest
    {
        public string KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public UserAddress Address { get; set; }
    }
}
