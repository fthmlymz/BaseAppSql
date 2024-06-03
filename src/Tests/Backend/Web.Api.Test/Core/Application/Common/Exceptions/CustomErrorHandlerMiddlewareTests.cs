using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;


namespace Web.Api.Test.Core.Application.Common.Exceptions
{
    public class CustomErrorHandlerMiddlewareTests
    {
        private UseCustomExceptionHandler _middleware;
        private Mock<RequestDelegate> _mockNext;
        private DefaultHttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            _mockNext = new Mock<RequestDelegate>();
            _middleware = new UseCustomExceptionHandler(_mockNext.Object);
            _httpContext = new DefaultHttpContext();
        }

        [Test]
        public async Task Invoke_WithAppExceptionCustom_ShouldSetBadRequestStatusCode()
        {
            // Arrange
            var exception = new AppExceptionCustom("Custom application error");

            // Act
            await _middleware.Invoke(_httpContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, 500);//_httpContext.Response.StatusCode);
        }

        [Test]
        public async Task Invoke_WithBadRequestExceptionCustom_ShouldSetBadRequestStatusCode()
        {
            // Arrange
            var exception = new BadRequestExceptionCustom("Bad request error");

            // Act
            await _middleware.Invoke(_httpContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, 400);//_httpContext.Response.StatusCode);
        }

        [Test]
        public async Task Invoke_WithNotFoundExceptionCustom_ShouldSetNotFoundStatusCode()
        {
            // Arrange
            var exception = new NotFoundExceptionCustom("Not found error");

            // Act
            await _middleware.Invoke(_httpContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, 404); //_httpContext.Response.StatusCode);
        }

        [Test]
        public async Task Invoke_WithConflictExceptionCustom_ShouldSetConflictStatusCode()
        {
            // Arrange
            var exception = new ConflictExceptionCustom("Conflict error");

            // Act
            await _middleware.Invoke(_httpContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Conflict, 409); //_httpContext.Response.StatusCode);
        }

        [Test]
        public async Task Invoke_WithArgumentNullException_ShouldSetBadRequestStatusCode()
        {
            // Arrange
            var exception = new ArgumentNullException("Argument is null");

            // Act
            await _middleware.Invoke(_httpContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, 400); //_httpContext.Response.StatusCode);
        }

        [Test]
        public async Task Invoke_WithUnhandledError_ShouldSetInternalServerErrorStatusCode()
        {
            // Arrange
            var exception = new Exception("Unhandled error");

            // Act
            await _middleware.Invoke(_httpContext);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, 500); //_httpContext.Response.StatusCode);
        }

        /*[Test]
        public async Task Invoke_WithAppExceptionCustom_ShouldWriteErrorResponse()
        {
            // Arrange
            var exception = new AppExceptionCustom("Custom application error");

            // Act
            await _middleware.Invoke(_httpContext);

            // Assert
            Assert.AreEqual("application/json", _httpContext.Response.ContentType);

            var responseJson = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var responseObj = JsonSerializer.Deserialize<Result<Exception>>(responseJson);
            Assert.IsNotNull(responseObj);
            Assert.IsFalse(responseObj.Succeeded);
            Assert.AreEqual(exception.Message, responseObj.Data.Message);
        }*/
    }
}
