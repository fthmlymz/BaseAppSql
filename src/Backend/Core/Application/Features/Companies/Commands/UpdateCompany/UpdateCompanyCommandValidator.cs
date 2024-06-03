using FluentValidation;

namespace Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("{PropertyName} alanı gereklidir.")
                .NotNull().WithMessage("{PropertyName} alanı boş gönderilemez.");

            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("{PropertyName} alanı gereklidir.")
                .NotNull().WithMessage("{PropertyName} alanı boş gönderilemez.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} alanı gereklidir.")
                .NotNull().WithMessage("{PropertyName} alanı boş gönderilemez.");

            RuleFor(x => x.UpdatedUserId)
                .NotNull().WithMessage("{PropertyName} bu alan gereklidir")
                .NotEmpty().WithMessage("{PropertyName} bu alan gereklidir")
                .MaximumLength(100).WithMessage("{PropertyName} alanı 100 karakteri geçemez.");

            RuleFor(x => x.UpdatedBy)
                .NotNull().WithMessage("{PropertyName} bu alan gereklidir")
                .NotEmpty().WithMessage("{PropertyName} bu alan gereklidir")
                .MaximumLength(100).WithMessage("{PropertyName} alanı 100 karakteri geçemez.");
        }
    }
}
