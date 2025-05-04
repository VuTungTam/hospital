using AutoMapper;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Specialties
{
    public class GetSpecialtyByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetSpecialtyByIdQuery, SpecialtyDto>
    {
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        public GetSpecialtyByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ISpecialtyReadRepository specialtyReadRepository
            ) : base(authService, mapper, localizer)
        {
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<SpecialtyDto> Handle(GetSpecialtyByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var result = await _specialtyReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (result == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            return _mapper.Map<SpecialtyDto>(result);
        }
    }
}
