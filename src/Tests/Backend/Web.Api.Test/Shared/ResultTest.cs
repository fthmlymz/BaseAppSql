using Shared;

namespace Web.Api.Test.Shared
{
    public class ResultTest
    {
        [Test]
        public void Success_MethodWithMessage_ReturnsSuccessResultWithMessage()
        {
            // Arrange
            string message = "Success message";

            // Act
            var result = Result<int>.Success(message);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.AreEqual(message, result.Messages[0]);
            Assert.AreEqual(default(int), result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Success_MethodWithData_ReturnsSuccessResultWithData()
        {
            // Arrange
            int data = 10;

            // Act
            var result = Result<int>.Success(data);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(0, result.Messages.Count);
            Assert.AreEqual(data, result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Success_MethodWithDataAndMessage_ReturnsSuccessResultWithDataAndMessage()
        {
            // Arrange
            int data = 10;
            string message = "Success message";

            // Act
            var result = Result<int>.Success(data, message);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.AreEqual(message, result.Messages[0]);
            Assert.AreEqual(data, result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Failure_Method_ReturnsFailureResult()
        {
            // Act
            var result = Result<int>.Failure();

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(0, result.Messages.Count);
            Assert.AreEqual(default(int), result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Failure_MethodWithMessage_ReturnsFailureResultWithMessage()
        {
            // Arrange
            string message = "Failure message";

            // Act
            var result = Result<int>.Failure(message);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.AreEqual(message, result.Messages[0]);
            Assert.AreEqual(default(int), result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Failure_MethodWithMessages_ReturnsFailureResultWithMessages()
        {
            // Arrange
            var messages = new List<string> { "Message 1", "Message 2" };

            // Act
            var result = Result<int>.Failure(messages);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(2, result.Messages.Count);
            Assert.AreEqual(messages[0], result.Messages[0]);
            Assert.AreEqual(messages[1], result.Messages[1]);
            Assert.AreEqual(default(int), result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Failure_MethodWithData_ReturnsFailureResultWithData()
        {
            // Arrange
            int data = 10;

            // Act
            var result = Result<int>.Failure(data);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(0, result.Messages.Count);
            Assert.AreEqual(data, result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Failure_MethodWithDataAndMessage_ReturnsFailureResultWithDataAndMessage()
        {
            // Arrange
            int data = 10;
            string message = "Failure message";

            // Act
            var result = Result<int>.Failure(data, message);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Messages.Count);
            Assert.AreEqual(message, result.Messages[0]);
            Assert.AreEqual(data, result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Failure_MethodWithDataAndMessages_ReturnsFailureResultWithDataAndMessages()
        {
            // Arrange
            int data = 10;
            var messages = new List<string> { "Message 1", "Message 2" };

            // Act
            var result = Result<int>.Failure(data, messages);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(2, result.Messages.Count);
            Assert.AreEqual(messages[0], result.Messages[0]);
            Assert.AreEqual(messages[1], result.Messages[1]);
            Assert.AreEqual(data, result.Data);
            Assert.IsNull(result.Exception);
            Assert.AreEqual(0, result.Code);
        }

        [Test]
        public void Failure_MethodWithException_ReturnsFailureResultWithException()
        {
            // Arrange
            var exception = new Exception("Test exception");

            // Act
            var result = Result<int>.Failure(exception);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(0, result.Messages.Count);
            Assert.AreEqual(default(int), result.Data);
            Assert.AreEqual(exception, result.Exception);
            Assert.AreEqual(0, result.Code);
        }
    }
}
