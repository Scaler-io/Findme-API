using API.Models.Requests.Photo;
using FluentValidation;

namespace API.Validators.Photo
{
    public class PhotoDeleteRequestValidator: AbstractValidator<PhotoDeleteRequest>
    {
        public PhotoDeleteRequestValidator()
        {
            RuleFor(photo => photo.PublicId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Public id is required");
        }
    }
}
