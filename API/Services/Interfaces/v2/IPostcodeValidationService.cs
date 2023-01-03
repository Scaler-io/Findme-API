using API.Models.Core;
using API.Models.Requests.Postcode;
using API.Models.Responses;

namespace API.Services.Interfaces.v2
{
    public interface IPostcodeValidationService
    {
        Task<Result<PostcodeValidationResponse>> ValidatePostcodeAsync(PostcodeSearchRequest request);
    }
}
