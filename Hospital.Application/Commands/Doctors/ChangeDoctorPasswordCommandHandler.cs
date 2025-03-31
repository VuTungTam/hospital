using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Security;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Doctors
{
    public class ChangeDoctorPasswordCommandHandler : BaseCommandHandler, IRequestHandler<ChangeDoctorPasswordCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly IDoctorWriteRepository _doctorWriteRepository;

        public ChangeDoctorPasswordCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IDoctorReadRepository doctorReadRepository,
            IDoctorWriteRepository doctorWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _doctorReadRepository = doctorReadRepository;
            _doctorWriteRepository = doctorWriteRepository;
        }

        public async Task<Unit> Handle(ChangeDoctorPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Dto.OldPassword))
            {
                throw new BadRequestException(_localizer["Account.OldPasswordMustNotBeEmpty"]);
            }

            if (string.IsNullOrEmpty(request.Dto.NewPassword))
            {
                throw new BadRequestException(_localizer["Account.NewPasswordMustNotBeEmpty"]);
            }

            await _authService.CheckPasswordLevelAndThrowAsync(request.Dto.NewPassword, cancellationToken);

            var doctor = await _doctorReadRepository.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);

            _authService.ValidateStateAndThrow(doctor);

            if (!PasswordHasher.Verify(request.Dto.OldPassword, doctor.PasswordHash))
            {
                throw new BadRequestException(_localizer["Account.OldPasswordIsIncorrect"]);
            }

            doctor.Password = request.Dto.NewPassword;
            doctor.IsDefaultPassword = false;
            doctor.IsPasswordChangeRequired = false;
            doctor.HashPassword();

            await _doctorWriteRepository.UpdateAsync(doctor, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
