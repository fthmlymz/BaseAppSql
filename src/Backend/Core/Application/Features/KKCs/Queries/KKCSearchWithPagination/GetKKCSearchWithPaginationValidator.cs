using FluentValidation;

namespace Application.Features.KKCs.Queries.KKCSearchWithPagination
{
    public class GetKKCSearchWithPaginationValidator : AbstractValidator<GetKKCSearchWithPaginationQuery>
    {
        public GetKKCSearchWithPaginationValidator()
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
