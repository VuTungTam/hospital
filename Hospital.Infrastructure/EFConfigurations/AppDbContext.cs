using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Hospital.Domain.Entities.Blogs;
using Hospital.Infra.EFConfigurations.EntityTypeConfigurations;
using Hospital.Infra.Extensions;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Hospital.SharedKernel.Libraries.Utils;
using System.Threading;
using Hospital.Domain.Entities.SocialNetworks;

namespace Hospital.Infra.EFConfigurations
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        private readonly IDateService _dateService;
        public AppDbContext(DbContextOptions options, IMediator mediator, IDateService dateService) : base(options)
        {
            _mediator = mediator;
            _dateService = dateService;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.CurrentTransaction ?? Database.BeginTransaction();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted);
            var timestamp = _dateService.GetClientTime();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IBaseEntity)
                        {
                            (entry.Entity as IBaseEntity).Id = AuthUtility.GenerateSnowflakeId();
                        }

                        if (entry.Entity is ICreated)
                        {
                            (entry.Entity as ICreated).Created = timestamp;
                        }

                        if (entry.Entity is IModified)
                        {
                            (entry.Entity as IModified).Modified = null;
                        }

                        

                        entry.State = EntityState.Added;
                        break;

                    case EntityState.Modified:
                        entry.Property(nameof(IBaseEntity.Id)).IsModified = false;
                        

                        if (entry.Entity is ICreated)
                        {
                            entry.Property(nameof(ICreated.Created)).IsModified = false;
                        }

                        if (entry.Entity is IModified)
                        {
                            (entry.Entity as IModified).Modified = timestamp;
                        }
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        public async Task CommitAsync(bool dispatch = true, CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
            await Database.CurrentTransaction.CommitAsync(cancellationToken);

            if (dispatch && false)
            {
                await _mediator.DispatchDomainEventsAsync(this, cancellationToken);
            }
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
    }
}
