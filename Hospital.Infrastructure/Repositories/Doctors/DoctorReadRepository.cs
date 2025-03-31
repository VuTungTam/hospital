using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Enums;
using Hospital.Domain.Specifications.Doctors;
using Hospital.Infrastructure.Extensions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Infrastructure.Redis;
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

        public async Task<PaginationResult<Doctor>> GetDoctorsPaginationResultAsync(Pagination pagination, List<long> specialtyIds, 
            AccountStatus status = AccountStatus.None, 
            DoctorDegree degree = DoctorDegree.None, DoctorTitle title = DoctorTitle.None, 
            DoctorRank rank = DoctorRank.None, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            var includable = query.Include(x => x.DoctorSpecialties)
                                  .ThenInclude(x => x.Specialty);

            ISpecification<Doctor> spec = new ExpressionSpecification<Doctor>(x => true); ;

            if (status != AccountStatus.None)
            {
                spec = spec.And( new DoctorByStatusEqualsSpecification(status));
            }

            if (specialtyIds.Any())
            {
                spec = spec.And(new DoctorBySpecialtyIdsEqualsSpecification(specialtyIds));
            }

            if (degree != DoctorDegree.None)
            {
                spec = spec.And(new DoctorByDegreeEqualsSpecification(degree));
            }

            if (title != DoctorTitle.None)
            {
                spec = spec.And(new DoctorByTitleEqualsSpecification(title));
            }

            if (rank != DoctorRank.None)
            {
                spec = spec.And(new DoctorByRankEqualsSpecification(rank));
            }

            query = query.Where(spec.GetExpression());

            query = query.BuildOrderBy(pagination.Sorts);

            var data = await query.ToListAsync(cancellationToken);
            

            var count = data.Count;

            data = data.Skip(pagination.Offset)
                       .Take(pagination.Size)
                       .ToList();

            return new PaginationResult<Doctor>(data, count);
        }

        public async Task<PaginationResult<Doctor>> GetPublicDoctorsPaginationResultAsync(Pagination pagination, List<long> specialtyIds,
            AccountStatus status = AccountStatus.None,
            DoctorDegree degree = DoctorDegree.None, DoctorTitle title = DoctorTitle.None,
            DoctorRank rank = DoctorRank.None, CancellationToken cancellationToken = default)
        {
            var query = BuildSearchPredicate(_dbSet.AsNoTracking(), pagination);

            query = query.Include(x => x.DoctorSpecialties)
                                  .ThenInclude(x => x.Specialty);

            ISpecification<Doctor> spec = new ExpressionSpecification<Doctor>(x => true); ;

            if (status != AccountStatus.None)
            {
                spec = spec.And(new DoctorByStatusEqualsSpecification(status));
            }

            if (specialtyIds.Any())
            {
                spec = spec.And(new DoctorBySpecialtyIdsEqualsSpecification(specialtyIds));
            }

            if (degree != DoctorDegree.None)
            {
                spec = spec.And(new DoctorByDegreeEqualsSpecification(degree));
            }

            if (title != DoctorTitle.None)
            {
                spec = spec.And(new DoctorByTitleEqualsSpecification(title));
            }

            if (rank != DoctorRank.None)
            {
                spec = spec.And(new DoctorByRankEqualsSpecification(rank));
            }

            query = query.Where(spec.GetExpression());

            query.Select(d => new Doctor
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
