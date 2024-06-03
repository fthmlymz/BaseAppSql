using Application.Common.Exceptions;
using FluentValidation.Results;

namespace Web.Api.Test.Core.Application.Common.Exceptions
{
    [TestFixture]
    public class BadRequestExceptionCustomTests
    {
        [Test]
        public void Constructor_WithErrorsArray_ShouldSetErrorsProperty()
        {
            // Arrange
            string[] errors = { "Error 1", "Error 2", "Error 3" };

            // Act
            var exception = new BadRequestExceptionCustom(errors);

            // Assert
            Assert.AreEqual(errors, exception.Errors);
        }

        [Test]
        public void Constructor_WithMessage_ShouldSetMessageProperty()
        {
            // Arrange
            string message = "Test exception message";

            // Act
            var exception = new BadRequestExceptionCustom(message);

            // Assert
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void Constructor_WithMessageAndInnerException_ShouldSetMessageAndInnerExceptionProperties()
        {
            // Arrange
            string message = "Test exception message";
            Exception innerException = new Exception("Inner exception message");

            // Act
            var exception = new BadRequestExceptionCustom(message, innerException);

            // Assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }

        [Test]
        public void Constructor_WithValidationFailures_ShouldSetFailuresProperty()
        {
            // Arrange
            var failures = new List<ValidationFailure>()
            {
                new ValidationFailure("Property1", "Error 1"),
                new ValidationFailure("Property2", "Error 2")
            };

            // Act
            var exception = new BadRequestExceptionCustom(failures);

            // Assert
            Assert.That(exception.Failures, Is.Not.Null);
            Assert.That(exception.Failures, Is.EquivalentTo(failures));
            Assert.AreEqual(failures, exception.Failures);
        }



        [Test]
        public void Constructor_WithErrorsList_ShouldSetValidationErrorsProperty()
        {
            // Arrange
            var errors = new List<string>() { "Error 1", "Error 2" };

            // Act
            var exception = new BadRequestExceptionCustom(errors);

            // Assert
            CollectionAssert.AreEqual(errors, exception.ValidationErrors);
        }

        [Test]
        public void Constructor_WithErrorsDictionary_ShouldNotThrowException()
        {
            // Arrange
            var errors = new Dictionary<string, string[]>
            {
                { "Property1", new[] { "Error 1" } },
                { "Property2", new[] { "Error 2", "Error 3" } }
            };

            // Act & Assert
            Assert.DoesNotThrow(() => new BadRequestExceptionCustom(errors));
        }
    }
}
