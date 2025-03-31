using AutoMapper;
using Hospital.Application.Commands.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Doctors
{
    public class UpdateDoctorProfileCommandHandler : BaseCommandHandler, IRequestHandler<UpdateDoctorProfileCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly IDoctorWriteRepository _doctorWriteRepository;
        private readonly IExecutionContext _executionContext;
        private readonly ILocationReadRepository _locationReadRepository;

        public UpdateDoctorProfileCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IUserRepository userRepository,
            IDoctorReadRepository doctorReadRepository,
            IDoctorWriteRepository doctorWriteRepository,
            IExecutionContext executionContext,
            ILocationReadRepository locationReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _userRepository = userRepository;
            _doctorReadRepository = doctorReadRepository;
            _doctorWriteRepository = doctorWriteRepository;
            _executionContext = executionContext;
            _locationReadRepository = locationReadRepository;
        }

        public async Task<Unit> Handle(UpdateDoctorProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _doctorReadRepository.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken) ?? throw new BadRequestException(_localizer["Account.NotFound"]);

            if (profile.Dob != null && request.Model.Dob > DateTime.Now)
            {
                throw new BadRequestException(_localizer["Authentication.DateOfBirthIsNotValid"]);
            }

            if (string.IsNullOrEmpty(request.Model.Email))
            {
                if (!string.IsNullOrEmpty(profile.Email))
                {
                    throw new BadRequestException(_localizer["Authentication.EmailMustNotBeEmpty"]);
                }
            }
            else
            {
                if (request.Model.Email != profile.Email)
                {
                    if (!EmailUtility.IsEmail(request.Model.Email))
                    {
                        throw new BadRequestException(_localizer["CommonMessage.EmailIsNotValid"]);
                    }

                    var existed = await _userRepository.EmailExistAsync(request.Model.Email, _executionContext.Identity, cancellationToken);
                    if (existed)
                    {
                        throw new BadRequestException(_localizer["Account.EmailAlreadyExists"]);
                    }

                    profile.Email = request.Model.Email;
                }
            }

            if (string.IsNullOrEmpty(request.Model.Phone))
            {
                if (!string.IsNullOrEmpty(profile.Phone))
                {
                    throw new BadRequestException(_localizer["Authentication.PhoneMustNotBeEmpty"]);
                }
            }
            else
            {
                if (request.Model.Phone != profile.Phone)
                {
                    if (!SmsUtility.IsVietnamesePhone(request.Model.Phone))
                    {
                        throw new BadRequestException(_localizer["Authentication.PhoneIsNotValid"]);
                    }

                    var existed = await _userRepository.PhoneExistAsync(request.Model.Phone, _executionContext.Identity, cancellationToken);
                    if (existed)
                    {
                        throw new BadRequestException(_localizer["Account.PhoneAlreadyExists"]);
                    }

                    profile.Phone = request.Model.Phone;
                }
            }

            profile.Name = request.Model.Name;
            profile.Dob = request.Model.Dob;
            profile.Pid = request.Model.Pid;
            profile.Did = request.Model.Did;
            profile.Wid = request.Model.Wid;
            profile.Address = request.Model.Address;

            profile.Pname = await _locationReadRepository.GetNameByIdAsync(profile.Pid, "province", cancellationToken);
            profile.Dname = await _locationReadRepository.GetNameByIdAsync(profile.Did, "district", cancellationToken);
            profile.Wname = await _locationReadRepository.GetNameByIdAsync(profile.Wid, "ward", cancellationToken);

            //_auditWriteRepository.Add(new Audit
            //{
            //    Type = AuditType.Account,
            //    Category = typeof(Doctor).Name.ToSnakeCaseLower(),
            //    Description = $"<p class='audit-normal'>Cập nhật thông tin tài khoản</strong></p>",
            //    AffectedId = profile.Id
            //});

            await _doctorWriteRepository.UpdateAsync(profile, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
