using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.SocialNetworks;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.Symptoms;
using Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.UnitOfWork;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Entites;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Entities;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Hospital.Infrastructure.Persistences.EF.EntityTypeConfigurations;
using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Entities.Distances;

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
            modelBuilder.ApplyConfiguration(new HealthFacilityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialtyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HealthServiceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HealthProfileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BookingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ActionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeActionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackEntityTypeConfiguration());
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

                        if (entry.Entity is ICreatedAt)
                        {
                            (entry.Entity as ICreatedAt).CreatedAt = timestamp;
                        }

                        if (entry.Entity is IModifiedAt)
                        {
                            (entry.Entity as IModifiedAt).ModifiedAt = null;
                        }

                        if (entry.Entity is IModifiedBy)
                        {
                            (entry.Entity as IModifiedBy).ModifiedBy = null;
                        }

                        if (entry.Entity is ISoftDelete)
                        {
                            (entry.Entity as ISoftDelete).DeletedAt = null;
                        }

                        if (entry.Entity is IDeletedBy)
                        {
                            (entry.Entity as IDeletedBy).DeletedBy = null;
                        }

                        if (!_executionContext.IsAnonymous)
                        {
                            if (entry.Entity is ICreatedBy)
                            {
                                (entry.Entity as ICreatedBy).CreatedBy = _executionContext.Identity;
                            }

                            if (entry.Entity is IOwnedEntity && (entry.Entity as IOwnedEntity).OwnerId == 0)
                            {
                                (entry.Entity as IOwnedEntity).OwnerId = _executionContext.Identity;
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

                        if (entry.Entity is ICreatedBy)
                        {
                            entry.Property(nameof(ICreatedBy.CreatedBy)).IsModified = false;
                        }

                        if (entry.Entity is ICreatedAt)
                        {
                            entry.Property(nameof(ICreatedAt.CreatedAt)).IsModified = false;
                        }

                        if (entry.Entity is ISoftDelete && entry.Entity is IDeletedBy && (entry.Entity as ISoftDelete).DeletedAt != null)
                        {
                            (entry.Entity as IDeletedBy).DeletedBy = _executionContext.Identity;
                        }
                        else
                        {
                            if (entry.Entity is IModifiedAt)
                            {
                                (entry.Entity as IModifiedAt).ModifiedAt = timestamp;
                            }
                            if (entry.Entity is IModifiedBy)
                            {
                                (entry.Entity as IModifiedBy).ModifiedBy = _executionContext.Identity;
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

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<RoleAction> RoleActions { get; set; }

        public DbSet<Sequence> Sequences { get; set; }

        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        public DbSet<LoginHistory> LoginHistories { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Distance> Distances { get; set; }
    }
}
