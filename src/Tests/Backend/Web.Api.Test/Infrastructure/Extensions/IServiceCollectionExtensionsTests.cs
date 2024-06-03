using Application.Interfaces.Services;
using Application.Interfaces;
using Domain.Common.Interfaces;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Web.Api.Test.Infrastructure.Extensions
{
    [TestFixture]
    public class IServiceCollectionExtensionsTests
    {
        [Test]
        public void AddInfrastructureLayer_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // or any other configuration method
                .Build();

            // Act
            services.AddInfrastructureLayer(configuration);

            // Assert
            AssertServiceRegistered<IMediator>(services);
            AssertServiceRegistered<IDomainEventDispatcher>(services);
            AssertServiceRegistered<IDateTimeService>(services);
            AssertServiceRegistered<IEasyCacheService>(services);
        }

        private void AssertServiceRegistered<TService>(IServiceCollection services)
        {
            var serviceDescriptor = services.GetDescriptor<TService>();
            Assert.IsNotNull(serviceDescriptor);
            Assert.AreEqual(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
            Assert.That(serviceDescriptor.Lifetime, Is.EqualTo(ServiceLifetime.Transient));
        }

        private class CacheSettings
        {
            // Define your CacheSettings class properties here
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static ServiceDescriptor GetDescriptor<TService>(this IServiceCollection services)
        {
            foreach (var serviceDescriptor in services)
            {
                if (serviceDescriptor.ServiceType == typeof(TService))
                {
                    return serviceDescriptor;
                }
            }

            return null;
        }
    }

    /*[TestFixture]
    public class IServiceCollectionExtensionsTests
    {
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void SetUp()
        {
            _configurationMock = new Mock<IConfiguration>();
        }

        [Test]
        public void AddInfrastructureLayer_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddInfrastructureLayer(_configurationMock.Object);

            // Assert
            Assert.That(services.Any<ServiceDescriptor>(s => s.ServiceType == typeof(IMediator)));
           // Assert.IsTrue(services.Any<ServiceDescriptor>(s => s.ServiceType == typeof(IDomainEventDispatcher)));
           // Assert.IsTrue(services.Any<ServiceDescriptor>(s => s.ServiceType == typeof(IDateTimeService)));
           // Assert.IsTrue(services.Any<ServiceDescriptor>(s => s.ServiceType == typeof(IEasyCacheService)));
        }
    }*/
}
