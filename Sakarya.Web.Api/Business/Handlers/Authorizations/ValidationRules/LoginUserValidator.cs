using Business.Services.Authentication.Model;
using FluentValidation;

namespace Business.Handlers.Authorizations.ValidationRules
{
    public class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(m => m.Password).NotEmpty();
            RuleFor(a => a.Email).EmailAddress().WithMessage("Invalid EMail");
        }
    }
}