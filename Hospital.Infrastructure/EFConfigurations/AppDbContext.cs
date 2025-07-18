﻿using System.Linq.Expressions;
using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.FacilityTypes;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.Images;
using Hospital.Domain.Entities.Payments;
using Hospital.Domain.Entities.ServiceTimeRules;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Entities.TimeSlots;
using Hospital.Domain.Entities.Zones;
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
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Action = Hospital.SharedKernel.Domain.Entities.Auths.Action;

namespace Hospital.Infrastructure.EFConfigurations
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
            modelBuilder.ApplyConfiguration(new EmployeeRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TimeSlotEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SystemConfigurationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ZoneEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MetaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ScriptEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ImageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceTimeRuleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CancelReasonEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ParameterExpression parameter = Expression.Parameter(entityType.ClrType);
                BinaryExpression combineExpression = null;

                if (entityType.ClrType.HasInterface<ISoftDelete>())
                {
                    var isDeletedProperty = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var isDeletedExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));

                    combineExpression = isDeletedExpression;
                }

                if (combineExpression != null)
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(combineExpression, parameter));
                }
            }
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
                if ((entry.Entity is IBaseEntity e) && e.Id == -1 && entry.State == EntityState.Modified)
                {
                    entry.State = EntityState.Added;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IBaseEntity)
                        {
                            if ((entry.Entity as IBaseEntity).Id == 0)
                            {
                                (entry.Entity as IBaseEntity).Id = AuthUtility.GenerateSnowflakeId();

                            }
                        }

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

        public DbSet<Province> Provinces { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Ward> Wards { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<HealthProfile> HealthProfiles { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<FacilitySpecialty> BrancSpecialties { get; set; }

        public DbSet<HealthFacility> HealthFacilities { get; set; }

        public DbSet<ServiceTimeRule> ServiceTimeRules { get; set; }

        public DbSet<TimeSlot> TimeSlots { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeRole> EmployeesRoles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<RoleAction> RoleActions { get; set; }

        public DbSet<Sequence> Sequences { get; set; }

        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        public DbSet<LoginHistory> LoginHistories { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Zone> Zones { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<ZoneSpecialty> ZoneSpecialties { get; set; }

        public DbSet<FacilityTypeMapping> FacilityTypeMappings { get; set; }

        public DbSet<FacilityType> FacilityTypes { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Action> Actions { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<FacilitySpecialty> FacilitySpecialties { get; set; }

        public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }

        public DbSet<HealthService> HealthServices { get; set; }
        public DbSet<Payment> Payments { get; set; }

    }
}
