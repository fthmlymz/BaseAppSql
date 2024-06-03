using Application.Features.Companies.Commands.CreateCompany;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.Logging;
using Moq;

namespace Web.Api.Test.Core.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyTest
    {
        private CreateCompanyCommandHandler _handler;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<CreateCompanyCommandHandler>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateCompanyCommandHandler>>();
            _handler = new CreateCompanyCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        #region CreateCompanyCommand

        #endregion

        #region Create Company
        [Test]
        public async Task Handle_ValidRequest_ShouldCreateCompanyAndReturnSuccessResult()
        {
            // Arrange
            var command = new CreateCompanyCommand("Name", "Description", 10, "CreatedBy", "CreatedUserId");

            var cancellationToken = new CancellationToken();

            var company = command.Adapt<Company>();
            _unitOfWorkMock.Setup(x => x.Repository<Company>().AddAsync(company)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsNotNull(result.Data);

            // Additional Assert methods
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.AreEqual(true, result.Succeeded);
            Assert.NotNull(result.Data);
        }
        #endregion
    }
}
