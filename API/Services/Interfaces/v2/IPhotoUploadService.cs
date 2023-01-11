using API.Models.Core;
using API.Models.Requests.Photo;
using API.Models.Responses;

namespace API.Services.Interfaces.v2
{
    public interface IPhotoUploadService
    {
        Task<Result<UserImageResponse>> UploadPhoto(PhotoUploadRequest request);
        Task<Result<bool>> DeletePhoto(PhotoDeleteRequest request);
        Task<Result<UserImageResponse>> UpdatePhotoAsMain(PhotoUpdateRequest request);
    }
}
 