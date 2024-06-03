using FluentValidation;

namespace Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} alanı gereklidir.")
                .NotNull().WithMessage("{PropertyName} alanı boş gönderilemez.")
                .MaximumLength(100).WithMessage("{PropertyName} alanı 100 karakteri geçemez.");

            RuleFor(p => p.TenantId)
                .NotEmpty().WithMessage("{PropertyName} alanı gereklidir.")
                .NotNull().WithMessage("{PropertyName} alanı boş gönderilemez.")
                .GreaterThan(0).WithMessage("{PropertyName} alanı 0 dan büyük olmalıdır.");

            RuleFor(p => p.CreatedBy)
                .MaximumLength(100).WithMessage("{PropertyName} alanı 100 karakteri geçemez.");

            RuleFor(p => p.CreatedUserId)
                .MaximumLength(100).WithMessage("{PropertyName} alanı 100 karakteri geçemez.");    
        }
    }
}
