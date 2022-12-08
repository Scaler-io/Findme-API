using API.Models.Requests.Account;
using FluentValidation;

namespace API.Validators.Account
{
    public class UserRegistrationValidator: AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationValidator()
        {
            RuleFor(acc => acc.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Username is required")
                .NotNull();

            RuleFor(acc => acc.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password length must be greater than 6")
                .Matches("((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{6,}))|((?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])(?=.{8,}))").WithMessage("Password is not strong");

            RuleFor(acc => acc.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .Equal(acc => acc.Password).WithMessage("Password did not match");

            RuleFor(acc => acc.Profile)
                .Cascade(CascadeMode.Stop)
                .SetValidator(new UserRegistrationProfileValidator());

            RuleFor(acc => acc.Address)
                .Cascade(CascadeMode.Stop)
                .SetValidator(new UserRegistrationAddressValidator());

        }
    }
}
