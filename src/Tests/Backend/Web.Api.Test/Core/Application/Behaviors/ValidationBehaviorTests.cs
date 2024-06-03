using Application.Behaviors;
using Application.Common.Exceptions;
using FluentValidation;
using MediatR;
using Moq;

namespace Web.Api.Test.Core.Application.Behaviors
{
    [TestFixture]
    public class ValidationBehaviorTests
    {
        private Mock<IValidator<TestRequest>> _validatorMock;
        private ValidationBehavior<TestRequest, TestResponse> _behavior;
        private Mock<RequestHandlerDelegate<TestResponse>> _nextMock;

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator<TestRequest>>();
            _behavior = new ValidationBehavior<TestRequest, TestResponse>(new List<IValidator<TestRequest>> { _validatorMock.Object });
            _nextMock = new Mock<RequestHandlerDelegate<TestResponse>>();
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldCallNext()
        {
            // Arrange
            var request = new TestRequest();
            _validatorMock.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>())).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            await _behavior.Handle(request, _nextMock.Object, CancellationToken.None);

            // Assert
            _nextMock.Verify(next => next(), Times.Once);
        }

        [Test]
        public void Handle_InvalidRequest_ShouldThrowBadRequestException()
        {
            // Arrange
            var request = new TestRequest();
            var failure = new FluentValidation.Results.ValidationFailure("Property", "Error message");
            var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { failure });
            _validatorMock.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>())).Returns(validationResult);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestExceptionCustom>(() => _behavior.Handle(request, _nextMock.Object, CancellationToken.None));
            Assert.Throws<BadRequestExceptionCustom>(() => _behavior.Handle(request, _nextMock.Object, CancellationToken.None));
            Assert.That(() => _behavior.Handle(request, _nextMock.Object, CancellationToken.None), Throws.TypeOf<BadRequestExceptionCustom>());
            Assert.Catch<BadRequestExceptionCustom>(() => _behavior.Handle(request, _nextMock.Object, CancellationToken.None));
            Assert.Throws<BadRequestExceptionCustom>(() => _behavior.Handle(request, _nextMock.Object, CancellationToken.None));
        }
    }

    public class TestRequest : IRequest<TestResponse> { }

    public class TestResponse { }
}
