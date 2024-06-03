using Application.Common.Exceptions;

namespace Web.Api.Test.Core.Application.Common.Exceptions
{
    [TestFixture]
    public class NotFoundExceptionCustomTests
    {
        [Test]
        public void NotFoundExceptionCustom_DefaultConstructor_ShouldCreateInstance()
        {
            // Arrange

            // Act
            var exception = new NotFoundExceptionCustom();

            // Assert
            Assert.IsNotNull(exception);
            Assert.IsInstanceOf<NotFoundExceptionCustom>(exception);
        }

        [Test]
        public void NotFoundExceptionCustom_MessageConstructor_ShouldCreateInstanceWithMessage()
        {
            // Arrange
            string message = "Test message";

            // Act
            var exception = new NotFoundExceptionCustom(message);

            // Assert
            Assert.IsNotNull(exception);
            Assert.IsInstanceOf<NotFoundExceptionCustom>(exception);
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void NotFoundExceptionCustom_MessageInnerExceptionConstructor_ShouldCreateInstanceWithMessageAndInnerException()
        {
            // Arrange
            string message = "Test message";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new NotFoundExceptionCustom(message, innerException);

            // Assert
            Assert.IsNotNull(exception);
            Assert.IsInstanceOf<NotFoundExceptionCustom>(exception);
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }

        [Test]
        public void NotFoundExceptionCustom_NameKeyConstructor_ShouldCreateInstanceWithFormattedMessage()
        {
            // Arrange
            string name = "TestEntity";
            object key = 123;

            // Act
            var exception = new NotFoundExceptionCustom(name, key);

            // Assert
            Assert.IsNotNull(exception);
            Assert.IsInstanceOf<NotFoundExceptionCustom>(exception);
            Assert.AreEqual($"Entity \"{name}\" ({key}) was not found.", exception.Message);
        }
    }
}
