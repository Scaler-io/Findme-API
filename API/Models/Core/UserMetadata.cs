namespace API.Models.Core
{
    public class UserMetadata
    {
        public int Id { get; set; }
        public string JoinedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string LastLogin { get; set; }
        public string CrteatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
