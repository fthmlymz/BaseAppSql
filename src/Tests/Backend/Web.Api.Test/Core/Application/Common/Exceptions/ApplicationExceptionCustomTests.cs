using Application.Common.Exceptions;

namespace Web.Api.Test.Core.Application.Common.Exceptions
{
    public class ApplicationExceptionCustomTests
    {
        [Test]
        public void Constructor_WithoutParameters_ShouldCreateInstance()
        {
            // Arrange

            // Act
            var exception = new ApplicationExceptionCustom();

            // Assert
            Assert.IsNotNull(exception);
        }

        [Test]
        public void Constructor_WithMessage_ShouldCreateInstanceWithMessage()
        {
            // Arrange
            string message = "Test Exception";

            // Act
            var exception = new ApplicationExceptionCustom(message);

            // Assert
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void Constructor_WithMessageAndInnerException_ShouldCreateInstanceWithMessageAndInnerException()
        {
            // Arrange
            string message = "Test Exception";
            var innerException = new System.Exception("Inner Exception");

            // Act
            var exception = new ApplicationExceptionCustom(message, innerException);

            // Assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }

        [Test]
        public void Constructor_WithMessageAndArgs_ShouldCreateInstanceWithFormattedMessage()
        {
            // Arrange
            string message = "Test Exception: {0}";
            string arg = "Argument";

            // Act
            var exception = new ApplicationExceptionCustom(message, arg);

            // Assert
            Assert.AreEqual(string.Format(message, arg), exception.Message);
        }
    }
}
