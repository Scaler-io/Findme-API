using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Addresses")]
    public class UserAddress: BaseEntity
    {
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile Profile { get; set; }
    }
}