using AutoMapper;
using Hospital.Application.Dtos.DoctorWorkingContexts;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetDoctorContextQueryHandler : BaseQueryHandler, IRequestHandler<GetDoctorContextQuery, DoctorWorkingContextDto>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IServiceTypeReadRepository _typeReadRepository;

        public GetDoctorContextQueryHandler(
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

        public async Task<DoctorWorkingContextDto> Handle(GetDoctorContextQuery request, CancellationToken cancellationToken)
        {
            if (request.DoctorId < 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var context = await _healthServiceReadRepository.GetServiceCurrentByDoctorIdAsync(request.DoctorId, cancellationToken);

            return context;
        }
    }
}
