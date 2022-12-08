using API.Models.Requests.Account;
using FluentValidation;

namespace API.Validators.Account
{
    public class UserRegistrationProfileValidator: AbstractValidator<UserProfileRequest>
    {
        public UserRegistrationProfileValidator()
        {
            RuleFor(acc => acc.Gender)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("'Gender' is required");

            RuleFor(acc => acc.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(acc => acc.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name is required");

            RuleFor(acc => acc.DateOfBirth)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("'Date of birth' is required");
            RuleFor(acc => acc.KnownAs)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("'Known as' field is required");
        }
    }
}
