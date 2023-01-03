using API.Models.Core.Postcode;
using API.Models.Responses;

namespace API.AutoMappers.Custom
{
    public static class PostcodeValidationMapper
    {
        public static PostcodeValidationResponse MapToValidationResponse(PostcodeSearchResponse postcCodeSearchResponse)
        {
            var postOfficeDetails = postcCodeSearchResponse.PostOffice.First();

            return new PostcodeValidationResponse
            {
                Status      = postcCodeSearchResponse.Status == "Success",
                City        = postOfficeDetails.Block != "NA" ? postOfficeDetails.Block : postOfficeDetails.Division,
                District    = postOfficeDetails.District,
                State       = postOfficeDetails.State,
            };
        }
    }
}
