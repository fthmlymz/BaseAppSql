using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the persistence layer to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the persistence layer to. Type: Microsoft.Extensions.DependencyInjection.IServiceCollection</param>
        /// <param name="configuration">The configuration object. Type: Microsoft.Extensions.Configuration.IConfiguration</param>
        /// <returns>void</returns>
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContexts(services, configuration);
            services.AddRepositories();
        }

        /// <summary>
        /// Adds the necessary DbContexts to the service collection and configures logging.
        /// </summary>
        /// <param name="services">The service collection to add DbContexts to.</param>
        /// <param name="configuration">The configuration object.</param>
        /// <returns>The updated service collection.</returns>
        private static void AddDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServerConnection");
            AddDbContext<ApplicationDbContext>(services, connectionString, typeof(ApplicationDbContext));

            var capConnectionString = configuration.GetConnectionString("CapLogSqlServerConnection");
            AddDbContext<DotnetCapDbContext>(services, capConnectionString, typeof(DotnetCapDbContext));

            var logger = CreateLogger(configuration);
            services.AddLogging(x => x.AddSerilog(logger));
        }

        /// <summary>
        /// Adds a database context to the service collection.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="services">The service collection to add the context to.</param>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <param name="assemblyType">The type from the assembly where migrations are located.</param>
        /// <returns>The modified service collection.</returns>
        private static void AddDbContext<TDbContext>(IServiceCollection services, string connectionString, Type assemblyType) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options =>
                options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly(assemblyType.Assembly.GetName().Name).MigrationsHistoryTable("__EFMigrationsHistory")));
        }

        /// <summary>
        /// Creates a logger with specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        /// <returns>The created logger.</returns>
        private static ILogger CreateLogger(IConfiguration configuration)
        {
            var serilogSeqUrl = configuration.GetSection("SerilogSeqUrl").Value;
            var serilogConnectionString = configuration.GetConnectionString("SeriLogConnection");
            var minimumLevel = configuration.GetValue<LogEventLevel>("Serilog:MinimumLevel:Default");

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(minimumLevel)
                .WriteTo.Seq(serilogSeqUrl)
                .WriteTo.MSSqlServer(
                    connectionString: serilogConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        AutoCreateSqlDatabase = true,
                        AutoCreateSqlTable = true,
                        TableName = "LogEvents"
                    });

            return loggerConfiguration.CreateLogger();
        }

        /// <summary>
        /// Adds repositories to the specified service collection.
        /// </summary>
        /// <param name="services">The service collection to add repositories to.</param>
        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
