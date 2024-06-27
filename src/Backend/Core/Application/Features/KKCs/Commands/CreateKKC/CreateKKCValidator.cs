using FluentValidation;

namespace Application.Features.KKCs.Commands.CreateKKC
{
    public class CreateKKCValidator : AbstractValidator<CreateKKCCommand>
    {
        public CreateKKCValidator()
        {
            RuleFor(x => x.DeviceId).NotNull().NotEmpty().WithMessage("{PropertyName} alanı gereklidir.")
;
            RuleFor(x => x.DeviceIp).NotNull().NotEmpty().WithMessage("{PropertyName} alanı gereklidir.");

            RuleFor(x => x.DevicePort).NotNull().NotEmpty().WithMessage("{PropertyName} alanı gereklidir.");

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("{PropertyName} alanı gereklidir.");

            RuleFor(x => x.Status).NotNull().NotEmpty().WithMessage("{PropertyName} alanı gereklidir.");

            RuleFor(x => x.CreatedBy).NotNull().NotEmpty().WithMessage("{PropertyName} alanı gereklidir.");

            RuleFor(x => x.CreatedUserId).NotNull().NotEmpty().WithMessage("{PropertyName} alanı gereklidir.");
        }
    }
}
