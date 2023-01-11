using Destructurama.Attributed;

namespace API.Models.Responses
{
    public class UserImageResponse
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        [LogMasked]
        public string PublicId { get; set; }
    }
}