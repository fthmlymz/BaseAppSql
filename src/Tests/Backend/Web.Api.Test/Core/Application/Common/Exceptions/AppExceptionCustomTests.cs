using Application.Common.Exceptions;

namespace Web.Api.Test.Core.Application.Common.Exceptions
{
    [TestFixture]
    public class AppExceptionCustomTests
    {
        [Test]
        public void AppExceptionCustom_DefaultConstructor_ShouldCreateInstance()
        {
            // Arrange

            // Act
            var exception = new AppExceptionCustom();

            // Assert
            Assert.IsNotNull(exception);
        }

        [Test]
        public void AppExceptionCustom_MessageConstructor_ShouldCreateInstanceWithMessage()
        {
            // Arrange
            string message = "Test Exception";

            // Act
            var exception = new AppExceptionCustom(message);

            // Assert
            Assert.IsNotNull(exception);
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void AppExceptionCustom_MessageArgsConstructor_ShouldCreateInstanceWithFormattedMessage()
        {
            // Arrange
            string message = "Test Exception: {0}";
            string arg = "Argument";

            // Act
            var exception = new AppExceptionCustom(message, arg);

            // Assert
            Assert.IsNotNull(exception);
            Assert.AreEqual(string.Format(message, arg), exception.Message);
        }
    }
}
