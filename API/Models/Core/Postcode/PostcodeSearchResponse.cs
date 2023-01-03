namespace API.Models.Core.Postcode
{
    public class PostcodeSearchResponse
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public List<PostOffice> PostOffice { get; set; }
    }
}
