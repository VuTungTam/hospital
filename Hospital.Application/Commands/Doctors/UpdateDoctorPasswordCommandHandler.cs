using AutoMapper;
using Hospital.Application.Commands.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Commands.Doctors
{
    public class UpdateDoctorPasswordCommandHandler : BaseCommandHandler, IRequestHandler<UpdateDoctorPasswordCommand>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly IDoctorWriteRepository _doctorWriteRepository;

        public UpdateDoctorPasswordCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IDoctorReadRepository doctorReadRepository,
            IDoctorWriteRepository doctorWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _doctorReadRepository = doctorReadRepository;
            _doctorWriteRepository = doctorWriteRepository;
        }

        public async Task<Unit> Handle(UpdateDoctorPasswordCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorReadRepository.GetByIdAsync(request.Model.UserId, cancellationToken: cancellationToken);
            if (doctor == null)
            {
                throw new BadRequestException("Tài khoản không tồn tại");
            }

            if (doctor.Status == AccountStatus.Blocked)
            {
                throw new BadRequestException("Tài khoản đã bị khóa");
            }

            await _authService.CheckPasswordLevelAndThrowAsync(request.Model.NewPassword, cancellationToken);

            doctor.Password = request.Model.NewPassword;
            doctor.IsDefaultPassword = false;
            doctor.IsPasswordChangeRequired = false;
            doctor.HashPassword();
            //doctor.AddDomainEvent(new UpdateDoctorPasswordDomainEvent(doctor));

            await _doctorWriteRepository.UpdateAsync(doctor, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
