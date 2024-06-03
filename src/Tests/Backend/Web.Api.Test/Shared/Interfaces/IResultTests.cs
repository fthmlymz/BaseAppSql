using Moq;
using Shared.Interfaces;

namespace Web.Api.Test.Shared.Interfaces
{

    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void ShouldInitializeWithDefaultValues()
        {
            // Arrange
            var result = new Mock<IResult<string>>();

            // Setup default values
            result.SetupGet(r => r.Messages).Returns(new List<string>());
            result.SetupGet(r => r.Succeeded).Returns(false);
            result.SetupGet(r => r.Data).Returns(default(string));
            result.SetupGet(r => r.Exception).Returns(default(Exception));
            result.SetupGet(r => r.Code).Returns(default(int));

            // Assert
            Assert.IsNotNull(result.Object.Messages);
            Assert.IsEmpty(result.Object.Messages);

            Assert.IsFalse(result.Object.Succeeded);

            Assert.IsNull(result.Object.Data);

            Assert.IsNull(result.Object.Exception);

            Assert.AreEqual(0, result.Object.Code);
        }

        [Test]
        public void ShouldSetValuesCorrectly()
        {
            // Arrange
            var messages = new List<string> { "Message 1", "Message 2" };
            var data = "Test Data";
            var exception = new Exception("Test Exception");
            var code = 500;

            var result = new Mock<IResult<string>>();

            // Setup default values
            result.SetupGet(r => r.Messages).Returns(new List<string>());
            result.SetupGet(r => r.Succeeded).Returns(true);
            result.SetupGet(r => r.Data).Returns(data);
            result.SetupGet(r => r.Exception).Returns(exception);
            result.SetupGet(r => r.Code).Returns(code);

            // Act
            result.Object.Messages.AddRange(messages);
            result.Object.Succeeded = true;
            result.Object.Data = data;
            result.Object.Exception = exception;
            result.Object.Code = code;

            // Assert
            Assert.IsNotNull(result.Object.Messages);
            Assert.AreEqual(messages, result.Object.Messages);
            Assert.IsTrue(result.Object.Succeeded);
            Assert.AreEqual(data, result.Object.Data);
            Assert.AreEqual(exception, result.Object.Exception);
            Assert.AreEqual(code, result.Object.Code);
        }
    }
}
