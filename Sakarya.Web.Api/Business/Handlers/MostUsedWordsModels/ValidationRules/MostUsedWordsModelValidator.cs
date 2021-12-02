using Business.Handlers.MostUsedWordsModels.Commands;
using FluentValidation;

namespace Business.Handlers.MostUsedWordsModels.ValidationRules
{
    public class CreateMostUsedWordsModelValidator : AbstractValidator<CreateMostUsedWordsModelCommand>
    {
        public CreateMostUsedWordsModelValidator()
        {
            RuleFor(x => x.DateTime).NotEmpty();
            RuleFor(x => x.Words).NotEmpty();
        }
    }
}