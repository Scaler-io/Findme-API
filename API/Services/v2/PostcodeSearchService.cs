using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Core.Postcode;
using API.Models.Requests.Postcode;
using API.Services.Interfaces.v2;
using Newtonsoft.Json;
using System.Net.Mime;
using ILogger = Serilog.ILogger;

namespace API.Services.v2
{
    public class PostcodeSearchService : IPostcodeSearchService
    {
        public readonly ILogger _logger;
        public readonly IHttpClientFactory _client;

        public PostcodeSearchService(ILogger logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory;
        }
        public async Task<Result<PostcodeSearchResponse>> PostcodeSearchAsync(PostcodeSearchRequest postcodeSearchRequest)
        {
            _logger.Here().MethoEnterd();
            _logger.Here().Information("PostcodeSearchAsync {@postcode}", postcodeSearchRequest.postCode);

            using (var client = _client.CreateClient(HttpClientNames.PostalValidationApi))
            {
                var routeParam = $"pincode/{postcodeSearchRequest.postCode}";
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, routeParam))
                {
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Json));
                    var httpResponse = await client.SendAsync(request);
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        _logger.Here().Error("{@ErrorCode} post code search failed", ErrorCodes.InternalServerError);
                        return Result<PostcodeSearchResponse>.Fail(ErrorCodes.InternalServerError);
                    }

                    var productSearchResponse = await httpResponse.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(productSearchResponse))
                    {
                        _logger.Here().Error("StatusCode:{0}",httpResponse.StatusCode.ToString());
                        return Result<PostcodeSearchResponse>.Fail(ErrorCodes.InternalServerError, productSearchResponse);
                    }

                    var response = JsonConvert.DeserializeObject<List<PostcodeSearchResponse>>(productSearchResponse);
                    _logger.Here().Information("post code search success {@response}", response[0]);
                    return Result<PostcodeSearchResponse>.Success(response[0]);
                }
            }
        }
    }
}
