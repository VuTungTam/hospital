using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Domain.Entities.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Doctors
{
    public class GetPublicDoctorByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetPublicDoctorByIdQuery, PublicDoctorDto>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly IRedisCache _redisCache;
        public GetPublicDoctorByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDoctorReadRepository doctorReadRepository,
            IRedisCache redisCache
        ) : base(authService, mapper, localizer)
        {
            _doctorReadRepository = doctorReadRepository;
            _redisCache = redisCache;
        }

        public async Task<PublicDoctorDto> Handle(GetPublicDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var doctor = await _doctorReadRepository.GetPublicDoctorById(request.Id, cancellationToken: cancellationToken);
            if (doctor == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var cacheKey = CacheManager.DbSystemPublicIdCacheEntry<Doctor>(request.Id);

            await _redisCache.SetAsync(cacheKey.Key, doctor, TimeSpan.FromSeconds(cacheKey.ExpiriesInSeconds), cancellationToken);

            var dto = _mapper.Map<PublicDoctorDto>(doctor);

            dto.ProfessionalLevel = string.Join(" - ", new[]
                {
                dto.DoctorTitle > 0 ? _localizer["DoctorTitle."+dto.DoctorTitle.ToString()] : null,
                dto.DoctorDegree > 0 ? _localizer["DoctorDegree."+dto.DoctorDegree.ToString()] : null,
                dto.DoctorRank > 0 ? _localizer["DoctorRank."+dto.DoctorRank.ToString()] : null,
                }.Where(x => !string.IsNullOrEmpty(x))) ?? _localizer["Infor.None"];

            return dto;
        }
    }
}
