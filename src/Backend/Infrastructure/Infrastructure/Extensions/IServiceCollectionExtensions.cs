﻿using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Common;
using Domain.Common.Interfaces;
using EasyCaching.Core.Configurations;
using Infrastructure.Services;
using Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMediator, Mediator>();
            services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddTransient<IDateTimeService, DateTimeService>();

            #region Redis
            var cacheSettings = configuration.GetSection("RedisConnectionSettings").Get<CacheSettings>();
            services.AddEasyCaching(option =>
            {
                option.WithJson();
                option.UseRedis(config =>
                {
                    config.DBConfig.ConnectionTimeout = cacheSettings.ConnectionTimeout;
                    config.DBConfig.Database = cacheSettings.Database;
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(cacheSettings.RedisURL, cacheSettings.RedisPort));
                }, "json");
            });
            services.AddTransient<IEasyCacheService, EasyCacheService>();
            #endregion
        }
    }
}
