using FluentValidation;

namespace Application.Features.KKCs.Commands.DeleteKKC
{
    public class DeleteKKCCommandValidator : AbstractValidator<DeleteKCCCommand>
    {
        public DeleteKKCCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("{PropertyName} bu alan gereklidir").NotEmpty().WithMessage("{PropertyName} bu alan gereklidir");
        }
    }
}
