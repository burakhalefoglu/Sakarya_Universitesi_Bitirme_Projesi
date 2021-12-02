using Business.Handlers.OperationClaims.Commands;
using FluentValidation;

namespace Business.Handlers.OperationClaims.ValidationRules
{
    public class CreateOperationClaimValidator : AbstractValidator<CreateOperationClaimCommand>
    {
        public CreateOperationClaimValidator()
        {
            RuleFor(x => x.Alias).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }

    public class UpdateOperationClaimValidator : AbstractValidator<UpdateOperationClaimCommand>
    {
        public UpdateOperationClaimValidator()
        {
            RuleFor(x => x.Alias).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}