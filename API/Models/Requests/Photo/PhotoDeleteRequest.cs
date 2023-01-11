using Destructurama.Attributed;

namespace API.Models.Requests.Photo
{
    public class PhotoDeleteRequest
    {
        [LogMasked(ShowLast = 5)]
        public string PublicId { get; set; }
    }
}
