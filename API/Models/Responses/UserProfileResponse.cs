namespace API.Models.Responses
{
    public class UserProfileResponse
    {
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
    }
}