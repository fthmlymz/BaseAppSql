using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;


namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly IConfiguration _configuration;

        public ApplicationDbContext()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="dispatcher">The domain event dispatcher.</param>
        /// <param name="configuration">The configuration.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcher dispatcher, IConfiguration configuration) : base(options)
        {
            _dispatcher = dispatcher;
            _configuration = configuration;
        }


        public DbSet<Company> Companies { get; set; }



        // Modifying the docstring to reflect the types
        /// <summary>
        /// This method overrides the base OnModelCreating method to configure the model using the provided modelBuilder.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance used to configure the model.</param>
        /// <returns>void</returns>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="dispatcher">The domain event dispatcher.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="optionsBuilder">The options builder for configuring the context.</param>
        /// <returns>Not returning any value.</returns>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServerConnection"));
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Saves changes asynchronously to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseAuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseAuditableEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseAuditableEntity)entityEntry.Entity).UpdatedDate = DateTime.UtcNow;
                }
            }

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            // ignore events if no dispatcher provided
            if (_dispatcher == null) return result;

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
