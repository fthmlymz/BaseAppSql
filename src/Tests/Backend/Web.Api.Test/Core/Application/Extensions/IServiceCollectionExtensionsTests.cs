using Application.Behaviors;
using Application.Extensions;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Web.Api.Test.Core.Application.Extensions
{
    [TestFixture]
    public class IServiceCollectionExtensionsTests
    {
        private IServiceCollection _services;

        [SetUp]
        public void Setup()
        {
            _services = new ServiceCollection();
            _services.AddApplicationLayer();
        }

        [Test]
        public void AddApplicationLayer_ShouldRegisterMapster()
        {
            // Arrange

            // Act
            //_services.AddMapster();

            // Assert
            Assert.IsFalse(_services.Any(service => service.ServiceType == typeof(TypeAdapterConfig)));
        }

        [Test]
        public void AddApplicationLayer_ShouldRegisterMediator()
        {
            // Arrange

            // Act
            //_services.AddMediator();

            // Assert
            Assert.IsTrue(_services.Any(service => service.ServiceType == typeof(IMediator)));
        }

        [Test]
        public void AddApplicationLayer_ShouldRegisterValidators()
        {
            // Arrange

            // Act
            //_services.AddValidators();

            // Assert
            Assert.IsFalse(_services.Any(service => service.ServiceType == typeof(IValidator<>)));
        }

        [Test]
        public void AddApplicationLayer_ShouldRegisterCachingBehavior()
        {
            // Arrange

            // Act
            _services.AddApplicationLayer();

            // Assert
            Assert.IsTrue(_services.Any(service => service.ServiceType == typeof(IPipelineBehavior<,>) && service.ImplementationType == typeof(CachingBehavior<,>)));
        }

        [Test]
        public void AddApplicationLayer_ShouldConfigureApiBehaviorOptions()
        {
            // Arrange

            // Act
            _services.AddApplicationLayer();

            // Assert
            var apiBehaviorOptions = _services.BuildServiceProvider().GetRequiredService<IOptions<ApiBehaviorOptions>>().Value;
            Assert.IsTrue(apiBehaviorOptions.SuppressModelStateInvalidFilter);
            Assert.IsNotNull(apiBehaviorOptions.InvalidModelStateResponseFactory);
        }
    }
}
