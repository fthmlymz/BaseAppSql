using FluentValidation.Results;

namespace Application.Common.Exceptions
{
    public class BadRequestExceptionCustom : Exception
    {
        //public List<ValidationFailure> Failures;
        public List<ValidationFailure> Failures { get; private set; }

        public string[] Errors { get; set; }
        public List<string> ValidationErrors { get; private set; } = new List<string>();

        public BadRequestExceptionCustom(IDictionary<string, string[]> errors) : base()
        {
        }
        public BadRequestExceptionCustom(List<string> errors) : base()
        {
            ValidationErrors = errors;
        }

        public BadRequestExceptionCustom(string message) : base(message)
        {
        }

        public BadRequestExceptionCustom(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BadRequestExceptionCustom(string[] errors) : base("Multiple errors occurred. See error details.")
        {
            Errors = errors;
        }

        public BadRequestExceptionCustom(List<ValidationFailure> failures)
        {
            Failures = failures;
        }
    }
}
