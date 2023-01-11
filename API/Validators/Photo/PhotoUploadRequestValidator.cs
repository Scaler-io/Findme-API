using API.Models.Requests.Photo;
using FluentValidation;

namespace API.Validators.Photo
{
    public class PhotoUploadRequestValidator: AbstractValidator<PhotoUploadRequest>
    {
        public PhotoUploadRequestValidator()
        {
            RuleFor(file => file.File)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Photo is required")
                .SetValidator(new FileExtensionValidator<PhotoUploadRequest, IFormFile>(2));
        }
    }
}
