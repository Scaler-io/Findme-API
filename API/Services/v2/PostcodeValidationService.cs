using API.AutoMappers.Custom;
using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Requests.Postcode;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using ILogger = Serilog.ILogger;

namespace API.Services.v2
{
    public class PostcodeValidationService : IPostcodeValidationService
    {
        public readonly ILogger _logger;
        private readonly IPostcodeSearchService _searchService;

        public PostcodeValidationService(ILogger logger, IPostcodeSearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        public async Task<Result<PostcodeValidationResponse>> ValidatePostcodeAsync(PostcodeSearchRequest postcodeSearchRequest)
        {
            _logger.Here().MethoEnterd();
            _logger.Here().Information("ValidatePostcodeAsync {@postcode}", postcodeSearchRequest.postCode);

            var validationResponse = await _searchService.PostcodeSearchAsync(postcodeSearchRequest);

            if (!validationResponse.IsSuccess || validationResponse.Value.Status == "Error")
            {
                _logger.Here().Error("postcode validation failed - {@validationResponse}", validationResponse);
                return Result<PostcodeValidationResponse>.Fail(ErrorCodes.InternalServerError, "post code validtaion failed");
            }

            var result = PostcodeValidationMapper.MapToValidationResponse(validationResponse.Value);

            _logger.Here().MethodExited();
            return Result<PostcodeValidationResponse>.Success(result);
        }
    }
}
