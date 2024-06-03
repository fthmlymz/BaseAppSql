using Application.Common.Exceptions;

namespace Web.Api.Test.Core.Application.Common.Exceptions
{
    public class ApiExceptionTests
    {
        [Test]
        public void ApiException_ConstructorWithStatusCodeAndErrors_ShouldSetProperties()
        {
            // Arrange
            int statusCode = 500;
            List<string> errors = new List<string> { "Error 1", "Error 2" };

            // Act
            ApiException exception = new ApiException(statusCode, errors);

            // Assert
            Assert.AreEqual(statusCode, exception.StatusCode);
            Assert.AreEqual(errors, exception.Errors);
        }

        [Test]
        public void ApiException_ConstructorWithStatusCodeAndError_ShouldSetProperties()
        {
            // Arrange
            int statusCode = 404;
            string error = "Not Found";

            // Act
            ApiException exception = new ApiException(statusCode, error);

            // Assert
            Assert.AreEqual(statusCode, exception.StatusCode);
            Assert.AreEqual(1, exception.Errors.Count);
            Assert.AreEqual(error, exception.Errors[0]);
        }
    }
}
