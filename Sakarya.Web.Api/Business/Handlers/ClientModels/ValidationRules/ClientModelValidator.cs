using Business.Handlers.ClientModels.Commands;
using FluentValidation;

namespace Business.Handlers.ClientModels.ValidationRules
{
    public class CreateClientModelValidator : AbstractValidator<CreateClientModelCommand>
    {
        public CreateClientModelValidator()
        {
            RuleFor(x => x.IsPleased).NotEmpty();
            RuleFor(x => x.UId).NotEmpty();
        }
    }
}