using API.Models.Requests.Photo;
using FluentValidation;

namespace API.Validators.Photo
{
    public class PhotoUpdateRequestValidator: AbstractValidator<PhotoUpdateRequest>
    {
        public PhotoUpdateRequestValidator()
        {
            RuleFor(photo => photo.PublicId)
                .NotEmpty()
                .WithMessage("Public id is required");
        }
    }
}
