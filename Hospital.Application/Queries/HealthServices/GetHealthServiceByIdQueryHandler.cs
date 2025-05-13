using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServiceByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthServiceByIdQuery, HealthServiceDto>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IServiceTypeReadRepository _typeReadRepository;

        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        public GetHealthServiceByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            ISpecialtyReadRepository specialtyReadRepository,
            IServiceTypeReadRepository typeReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _specialtyReadRepository = specialtyReadRepository;
            _typeReadRepository = typeReadRepository;
        }

        public async Task<HealthServiceDto> Handle(GetHealthServiceByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthService.ServiceTimeRules) }
            };

            var service = await _healthServiceReadRepository.GetByIdAsync(request.Id, option: option, cancellationToken: cancellationToken);

            if (service == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var facility = await _healthFacilityReadRepository.GetByIdAsync(service.FacilityId, cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var specialty = await _specialtyReadRepository.GetByIdAsync(service.SpecialtyId, cancellationToken: cancellationToken);

            if (specialty == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var type = await _typeReadRepository.GetByIdAsync(service.TypeId, cancellationToken: cancellationToken);

            if (type == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var serviceDto = _mapper.Map<HealthServiceDto>(service);

            serviceDto.FacilityNameVn = facility.NameVn;

            serviceDto.FacilityNameEn = facility.NameEn;

            serviceDto.TypeNameVn = type.NameVn;

            serviceDto.TypeNameEn = type.NameEn;

            serviceDto.SpecialtyNameEn = specialty.NameEn;

            serviceDto.SpecialtyNameVn = specialty.NameVn;

            var address = facility.Address + ',' + facility.Wname + ',' + facility.Dname + ',' + facility.Pname;

            serviceDto.FacilityFullAddress = address;

            return serviceDto;
        }
    }
}
