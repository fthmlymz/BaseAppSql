using Application.Common.Exceptions;

namespace Web.Api.Test.Core.Application.Common.Exceptions
{
    public class ConflictExceptionCustomTests
    {
        [Test]
        public void Constructor_WithoutParameters_ShouldCreateInstance()
        {
            // Arrange

            // Act
            var exception = new ConflictExceptionCustom();

            // Assert
            Assert.IsNotNull(exception);
        }

        [Test]
        public void Constructor_WithMessage_ShouldCreateInstanceWithMessage()
        {
            // Arrange
            string message = "Test message";

            // Act
            var exception = new ConflictExceptionCustom(message);

            // Assert
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void Constructor_WithMessageAndArgs_ShouldCreateInstanceWithFormattedMessage()
        {
            // Arrange
            string message = "Test message with args: {0} and {1}";
            object[] args = { "arg1", "arg2" };
            string expectedMessage = string.Format(message, args);

            // Act
            var exception = new ConflictExceptionCustom(message, args);

            // Assert
            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}
