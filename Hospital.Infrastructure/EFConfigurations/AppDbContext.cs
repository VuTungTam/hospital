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
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;
using Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Domain.Entities.Declarations;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.Domain.Entities.QueueItems;

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
            modelBuilder.ApplyConfiguration(new SocialNetworkEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SymptomEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HealthFacilityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialtyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HealthServiceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DeclarationEntityTypeConfiguration());
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
                        //if (entry.Entity is IBaseEntity)
                        //{
                            //(entry.Entity as IBaseEntity).Id = AuthUtility.GenerateSnowflakeId();
                        //}

                        if (entry.Entity is ICreated)
                        {
                            (entry.Entity as ICreated).Created = timestamp;
                        }
                        if (entry.Entity is IModifier)
                        {
                            (entry.Entity as IModifier).Modifier = null;
                        }
                        if (entry.Entity is IModified)
                        {
                            (entry.Entity as IModified).Modified = null;
                        }
                        if (entry.Entity is ISoftDelete)
                        {
                            (entry.Entity as ISoftDelete).Deleted = null;
                        }

                        if (entry.Entity is IDeletedBy)
                        {
                            (entry.Entity as IDeletedBy).DeletedBy = null;
                        }
                        entry.State = EntityState.Added;
                        break;

                    case EntityState.Modified:
                        entry.Property(nameof(IBaseEntity.Id)).IsModified = false;


                        if (entry.Entity is ICreated)
                        {
                            entry.Property(nameof(ICreated.Created)).IsModified = false;
                        }

                        if (entry.Entity is ICreator)
                        {
                            entry.Property(nameof(ICreator.Creator)).IsModified = false;
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
            if(Database.CurrentTransaction == null)
            {
                BeginTransaction();
            }
            await Database.CurrentTransaction.CommitAsync(cancellationToken);

            if (dispatch && false)
            {
                await _mediator.DispatchDomainEventsAsync(this, cancellationToken);
            }
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Declaration> Declarations { get; set; }
        public DbSet<DeclarationSymptom> DeclarationSymptoms { get; set; }
        public DbSet<QueueItem> QueueItems { get; set; }
    }
}
