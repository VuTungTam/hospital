using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Doctors
{
    public class GetDoctorByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetDoctorByIdQuery, DoctorDto>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;

        public GetDoctorByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDoctorReadRepository doctorReadRepository
        ) : base(authService, mapper, localizer)
        {
            _doctorReadRepository = doctorReadRepository;
        }

        public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var doctor = await _doctorReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (doctor == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}
