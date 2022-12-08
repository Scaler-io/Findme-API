using API.Models.Requests.Account;
using FluentValidation;

namespace API.Validators.Account
{
    public class UserRegistrationAddressValidator : AbstractValidator<UserAddressRequest>
    {
        public UserRegistrationAddressValidator()
        {
            RuleFor(acc => acc.StreetNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("street number is required");
            RuleFor(acc => acc.StreetName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("street name is required");
            RuleFor(acc => acc.StreetType)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("street type is required");
            RuleFor(acc => acc.City)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("city is required");
            RuleFor(acc => acc.District)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("district is required");
            RuleFor(acc => acc.State)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("state is required");
            RuleFor(acc => acc.PostCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("post code is required");
        }
    }
}