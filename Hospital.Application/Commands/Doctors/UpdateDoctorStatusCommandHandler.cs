using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Doctors
{
    public class UpdateDoctorAccountStatusCommandHandler : BaseCommandHandler, IRequestHandler<UpdateDoctorStatusCommand>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly IDoctorWriteRepository _doctorWriteRepository;
        private readonly IAuthRepository _authRepository;

        public UpdateDoctorAccountStatusCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IDoctorReadRepository doctorReadRepository,
            IDoctorWriteRepository doctorWriteRepository,
            IAuthRepository authRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _doctorReadRepository = doctorReadRepository;
            _doctorWriteRepository = doctorWriteRepository;
            _authRepository = authRepository;
        }

        public async Task<Unit> Handle(UpdateDoctorStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            if (request.Status == AccountStatus.None)
            {
                return Unit.Value;
            }

            var doctor = await _doctorReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (doctor == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            if (doctor.Status == request.Status)
            {
                return Unit.Value;
            }

            var oldStatus = doctor.Status;

            doctor.Status = request.Status;
            if (doctor.Status != AccountStatus.Active)
            {
                await _authRepository.RemoveRefreshTokensAsync(new List<long> { doctor.Id }, cancellationToken);
            }

            //doctor.AddDomainEvent(new UpdateDoctorStatusDomainEvent(doctor, oldStatus));

            await _doctorWriteRepository.UpdateAsync(doctor, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
