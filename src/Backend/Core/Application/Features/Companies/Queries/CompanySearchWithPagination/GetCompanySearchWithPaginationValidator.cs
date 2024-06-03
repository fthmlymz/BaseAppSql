using FluentValidation;

namespace Application.Features.Companies.Queries.CompanySearchWithPagination
{
    public class GetCompanySearchWithPaginationValidator : AbstractValidator<GetCompanySearchWithPaginationQuery>
    {
        public GetCompanySearchWithPaginationValidator()
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
