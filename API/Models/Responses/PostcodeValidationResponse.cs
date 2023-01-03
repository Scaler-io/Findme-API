namespace API.Models.Responses
{
    public class PostcodeValidationResponse
    {
        public bool Status { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
    }
}
