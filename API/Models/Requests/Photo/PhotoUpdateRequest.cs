using Destructurama.Attributed;

namespace API.Models.Requests.Photo
{
    public class PhotoUpdateRequest
    {
        [LogMasked(ShowLast = 5)]
        public string PublicId { get; set; }
    }
}
