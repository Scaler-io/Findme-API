using API.Extensions.Utils;
using API.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Profiles")]
    public class UserProfile: BaseEntity
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public Gender Gender { get; set; }
        public string Bio { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public UserAddress Address { get; set; }
        public ICollection<UserImage> Photos { get; set; } = new List<UserImage>();
        public int AppUserId { get; set; }
        public AppUser User { get; set; }

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}
