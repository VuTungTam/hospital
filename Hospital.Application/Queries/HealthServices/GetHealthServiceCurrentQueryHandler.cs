using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServiceCurrentQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthServiceCurrentQuery, PaginationResult<HealthServiceDto>>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IServiceTypeReadRepository _typeReadRepository;

        public GetHealthServiceCurrentQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository,
            ISpecialtyReadRepository specialtyReadRepository,
            IServiceTypeReadRepository typeReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
            _specialtyReadRepository = specialtyReadRepository;
            _typeReadRepository = typeReadRepository;
        }

        public async Task<PaginationResult<HealthServiceDto>> Handle(GetHealthServiceCurrentQuery request, CancellationToken cancellationToken)
        {
            if (request.DoctorId < 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var services = await _healthServiceReadRepository.GetServiceCurrentAsync(request.Pagination, request.FacilityId, request.DoctorId, cancellationToken);

            var serviceDtos = new List<HealthServiceDto>();

            foreach (var service in services.Data)
            {
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

                serviceDto.TypeNameVn = type.NameVn;

                serviceDto.TypeNameEn = type.NameEn;

                serviceDto.SpecialtyNameEn = specialty.NameEn;

                serviceDto.SpecialtyNameVn = specialty.NameVn;

                serviceDtos.Add(serviceDto);
            }

            return new PaginationResult<HealthServiceDto>(serviceDtos, services.Total);
        }
    }
}
