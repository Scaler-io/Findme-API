namespace API.Models.Responses
{
    public class UserProfileResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
    }
}