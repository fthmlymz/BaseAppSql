using FluentValidation;

namespace Application.Features.Companies.Queries.CompanyPaginatedAllList
{
    public class GetCompanyPaginatedAllListValidator : AbstractValidator<GetCompanyPaginatedAllListQuery>
    {
        public GetCompanyPaginatedAllListValidator()
        {
            RuleFor(x => x.PageNumber)
              .NotEmpty().WithMessage("{PropertyName} alanı gereklidir.")
              .NotNull().WithMessage("{PropertyName} alanı boş gönderilemez.");

            RuleFor(x => x.PageSize)
                .NotEmpty().WithMessage("{PropertyName} alanı gereklidir.")
                .NotNull().WithMessage("{PropertyName} alanı boş gönderilemez.");
        }
    }
}
