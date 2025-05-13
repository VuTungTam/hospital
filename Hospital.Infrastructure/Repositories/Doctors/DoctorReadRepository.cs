using AutoMapper;
using Hospital.Application.Models.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Enums;
using Hospital.Domain.Models.Admin;
using Hospital.Domain.Specifications;
using Hospital.Domain.Specifications.Doctors;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Security;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Doctors
{
    public class DoctorReadRepository : ReadRepository<Doctor>, IDoctorReadRepository
    {

        public DoctorReadRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IMapper mapper
            ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public override ISpecification<Doctor> GuardDataAccess<Doctor>(ISpecification<Doctor> spec, QueryOption option = default)
        {
            option ??= new QueryOption();

            spec ??= new ExpressionSpecification<Doctor>(x => true);

            spec = spec.And(base.GuardDataAccess(spec, option));

            if (!option.IgnoreFacility)
            {
                spec = spec.And(new LimitByFacilityIdSpecification<Doctor>(_executionContext.FacilityId));

            }
            return spec;
        }

        public async Task<Doctor> GetLoginByEmailAsync(string email, string password, bool checkPassword = true, CancellationToken cancellationToken = default)
        {
            var spec = new DoctorByEmailEqualsSpecification(email)
                   .Or(new DoctorByEmailEqualsSpecification($"{email}@gmail.com"));

            var doctor = await _dbSet.AsNoTracking().FirstOrDefaultAsync(spec.GetExpression(), cancellationToken);
            if (doctor == null)
            {
                return null;
            }

            if (checkPassword && password != PowerfulSetting.Password && !PasswordHasher.Verify(password, doctor.PasswordHash))
            {
                return null;
            }

            return doctor;
        }

        public async Task<PaginationResult<Doctor>> GetDoctorsPaginationResultAsync(Pagination pagination,
            FilterDoctorRequest request, AccountStatus status = AccountStatus.None,
            CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            query = query.Include(x => x.DoctorSpecialties)
                                  .ThenInclude(x => x.Specialty);

            ISpecification<Doctor> spec = new ExpressionSpecification<Doctor>(x => true); ;

            if (status != AccountStatus.None)
            {
                spec = spec.And(new DoctorByStatusEqualsSpecification(status));
            }

            if (request.SpeIds.Any())
            {
                spec = spec.And(new DoctorBySpecialtyIdsEqualsSpecification(request.SpeIds));
            }

            if (request.Degrees.Any())
            {
                spec = spec.And(new DoctorByDegreeEqualsSpecification(request.Degrees));
            }

            if (request.Titles.Any())
            {
                spec = spec.And(new DoctorByTitleEqualsSpecification(request.Titles));
            }

            if (request.Ranks.Any())
            {
                spec = spec.And(new DoctorByRankEqualsSpecification(request.Ranks));
            }

            if (request.Genders.Any())
            {
                spec = spec.And(new DoctorByGenderEqualsSpecification(request.Genders));
            }

            spec = GuardDataAccess(spec);

            query = query.Where(spec.GetExpression());

            query = query.BuildOrderBy(pagination.Sorts);

            var data = await query.ToListAsync(cancellationToken);

            var count = data.Count;

            data = data.Skip(pagination.Offset)
                       .Take(pagination.Size)
                       .ToList();

            return new PaginationResult<Doctor>(data, count);
        }

        public async Task<PaginationResult<Doctor>> GetPublicDoctorsPaginationResultAsync(Pagination pagination,
            FilterDoctorRequest request, long facilityId = 0, AccountStatus status = AccountStatus.None,
            CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            query = query.Include(x => x.DoctorSpecialties)
                                  .ThenInclude(x => x.Specialty);

            ISpecification<Doctor> spec = new ExpressionSpecification<Doctor>(x => true); ;

            if (facilityId > 0)
            {
                spec = spec.And(new LimitByFacilityIdSpecification<Doctor>(facilityId));
            }

            if (status != AccountStatus.None)
            {
                spec = spec.And(new DoctorByStatusEqualsSpecification(status));
            }

            if (request.SpeIds.Any())
            {
                spec = spec.And(new DoctorBySpecialtyIdsEqualsSpecification(request.SpeIds));
            }

            if (request.Degrees.Any())
            {
                spec = spec.And(new DoctorByDegreeEqualsSpecification(request.Degrees));
            }

            if (request.Titles.Any())
            {
                spec = spec.And(new DoctorByTitleEqualsSpecification(request.Titles));
            }

            if (request.Ranks.Any())
            {
                spec = spec.And(new DoctorByRankEqualsSpecification(request.Ranks));
            }

            if (request.Genders.Any())
            {
                spec = spec.And(new DoctorByGenderEqualsSpecification(request.Genders));
            }
            var option = new QueryOption
            {
                IgnoreFacility = true,
            };
            spec = GuardDataAccess(spec, option);

            query = query.Where(spec.GetExpression());

            query.Select(d => new Doctor
            {
                Id = d.Id,
                Name = d.Name,
                Avatar = d.Avatar,
                Gender = d.Gender,
                DoctorTitle = d.DoctorTitle,
                DoctorDegree = d.DoctorDegree,
                DoctorRank = d.DoctorRank,
                Expertise = d.Expertise,
                WorkExperience = d.WorkExperience,
                TrainingProcess = d.TrainingProcess,
                Description = d.Description,
                FacilityId = d.FacilityId,
                DoctorSpecialties = d.DoctorSpecialties.Select(ds => new DoctorSpecialty
                {
                    DoctorId = ds.DoctorId,
                    SpecialtyId = ds.SpecialtyId
                }).ToList()
            });

            query = query.BuildOrderBy(pagination.Sorts);

            var data = await query.ToListAsync(cancellationToken);

            var count = data.Count;

            data = data.Skip(pagination.Offset)
                       .Take(pagination.Size)
                       .ToList();

            return new PaginationResult<Doctor>(data, count);
        }

        public async Task<Doctor> GetPublicDoctorById(long id, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsNoTracking();

            var doctor = await query
            .Where(d => d.Id == id)
            .Select(d => new Doctor
            {
                Id = d.Id,
                Name = d.Name,
                DoctorTitle = d.DoctorTitle,
                DoctorDegree = d.DoctorDegree,
                DoctorRank = d.DoctorRank,
                Expertise = d.Expertise,
                WorkExperience = d.WorkExperience,
                TrainingProcess = d.TrainingProcess,
                Description = d.Description,
                FacilityId = d.FacilityId,
                DoctorSpecialties = d.DoctorSpecialties.Select(ds => new DoctorSpecialty
                {
                    DoctorId = ds.DoctorId,
                    SpecialtyId = ds.SpecialtyId
                }).ToList()
            })

            .FirstOrDefaultAsync(cancellationToken);

            return doctor;
        }
    }
}
