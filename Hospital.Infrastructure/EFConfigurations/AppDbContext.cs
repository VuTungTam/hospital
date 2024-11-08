using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.Newses;
using Hospital.Domain.Entities.QueueItems;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Infra.Extensions;
using Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations;
using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Hospital.SharedKernel.Domain.Entities.Users;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Entities;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hospital.Infra.EFConfigurations
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IExecutionContext _executionContext;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IDateService _dateService;
        public AppDbContext(
           DbContextOptions<AppDbContext> options,
           IExecutionContext executionContext,
           IEventDispatcher eventDispatcher,
           IDateService dateService
       ) : base(options)
        {
            _executionContext = executionContext;
            _eventDispatcher = eventDispatcher;
            _dateService = dateService;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SocialNetworkEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SymptomEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NewsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HealthFacilityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialtyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HealthServiceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HealthProfileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new QueueItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BookingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BranchEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserBranchEntityTypeConfiguration());
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
                        //    (entry.Entity as IBaseEntity).Id = AuthUtility.GenerateSnowflakeId();
                        //}

                        if (entry.Entity is ICreated)
                        {
                            (entry.Entity as ICreated).Created = timestamp;
                        }

                        if (entry.Entity is IModified)
                        {
                            (entry.Entity as IModified).Modified = null;
                        }

                        if (entry.Entity is IModifier)
                        {
                            (entry.Entity as IModifier).Modifier = null;
                        }

                        if (entry.Entity is ISoftDelete)
                        {
                            (entry.Entity as ISoftDelete).Deleted = null;
                        }

                        if (entry.Entity is IDeletedBy)
                        {
                            (entry.Entity as IDeletedBy).DeletedBy = null;
                        }

                        if (!_executionContext.IsAnonymous)
                        {
                            if (entry.Entity is IBranchId && (entry.Entity as IBranchId).BranchId == 0)
                            {
                                (entry.Entity as IBranchId).BranchId = _executionContext.BranchId;
                            }

                            if (entry.Entity is ICreator)
                            {
                                (entry.Entity as ICreator).Creator = _executionContext.UserId;
                            }

                            if (entry.Entity is IPersonalizeEntity && (entry.Entity as IPersonalizeEntity).OwnerId == 0)
                            {
                                (entry.Entity as IPersonalizeEntity).OwnerId = _executionContext.UserId;
                            }
                        }

                        entry.State = EntityState.Added;
                        break;

                    case EntityState.Modified:
                        entry.Property(nameof(IBaseEntity.Id)).IsModified = false;

                        //if (entry.Entity is IPersonalizeEntity persionalizedEntity)
                        //{
                        //    entry.Property(nameof(IPersonalizeEntity.OwnerId)).IsModified = false;
                        //}

                        if (entry.Entity is IBranchId)
                        {
                            entry.Property(nameof(IBranchId.BranchId)).IsModified = false;
                        }

                        if (entry.Entity is ICreator)
                        {
                            entry.Property(nameof(ICreator.Creator)).IsModified = false;
                        }

                        if (entry.Entity is ICreated)
                        {
                            entry.Property(nameof(ICreated.Created)).IsModified = false;
                        }

                        if (entry.Entity is ISoftDelete && entry.Entity is IDeletedBy && (entry.Entity as ISoftDelete).Deleted != null)
                        {
                            (entry.Entity as IDeletedBy).DeletedBy = _executionContext.UserId;
                        }
                        else
                        {
                            if (entry.Entity is IModified)
                            {
                                (entry.Entity as IModified).Modified = timestamp;
                            }
                            if (entry.Entity is IModifier)
                            {
                                (entry.Entity as IModifier).Modifier = _executionContext.UserId;
                            }
                        }
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        public async Task CommitAsync(bool dispatchDomainEvents = true, CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
            await Database.CurrentTransaction.CommitAsync(cancellationToken);

            if (!dispatchDomainEvents)
            {
                return;
            }

            var entries = ChangeTracker.Entries<BaseEntity>().Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
            var events = entries.SelectMany(x => x.Entity.DomainEvents).ToList();

            if (!events.Any())
            {
                return;
            }

            foreach (var entity in entries)
            {
                entity.Entity.ClearDomainEvents();
            }

            var tasks = events.Select(@event => _eventDispatcher.FireEventAsync(@event, cancellationToken));
            await Task.WhenAll(tasks);
        }

        public DbSet<SocialNetwork> SocialNetworks { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Ward> Wards { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<BookingSymptom> BookingSymptoms { get; set; }

        public DbSet<Symptom> Symptoms { get; set; }

        public DbSet<HealthProfile> HealthProfiles { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<FacilitySpecialty> BrancSpecialties { get; set; }

        public DbSet<QueueItem> QueueItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserBranch> UserBranches { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<RoleAction> RoleActions { get; set; }

        public DbSet<Sequence> Sequences { get; set; }

        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        public DbSet<News> News { get; set; }
    }
}
