using API.Models.Core;
using API.Models.Core.Postcode;
using API.Models.Requests.Postcode;

namespace API.Services.Interfaces.v2
{
    public interface IPostcodeSearchService
    {
        Task<Result<PostcodeSearchResponse>> PostcodeSearchAsync(PostcodeSearchRequest request);
    }
}
