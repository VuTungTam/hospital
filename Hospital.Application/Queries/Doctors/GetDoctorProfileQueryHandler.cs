using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Doctors
{
    public class GetDoctorProfileQueryHandler : BaseQueryHandler, IRequestHandler<GetDoctorProfileQuery, DoctorDto>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly IExecutionContext _executionContext;
        public GetDoctorProfileQueryHandler(
            IAuthService authService, 
            IMapper mapper, 
            IDoctorReadRepository doctorReadRepository,
            IExecutionContext executionContext,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _doctorReadRepository = doctorReadRepository;
            _executionContext = executionContext;
        }

        public async Task<DoctorDto> Handle(GetDoctorProfileQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorReadRepository.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);
            if (doctor == null)
            {
                return null;
            }
            var dto = _mapper.Map<DoctorDto>(doctor);

            return dto;
        }
    }
}
