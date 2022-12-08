namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }      
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public UserProfile Profile { get; set; } = new UserProfile();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}