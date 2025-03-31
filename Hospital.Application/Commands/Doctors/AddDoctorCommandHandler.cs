using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Doctors
{
    public class AddDoctorCommandHandler : BaseCommandHandler, IRequestHandler<AddDoctorCommand, string>
    {
        private readonly IDoctorWriteRepository _doctorWriteRepository;
        private readonly ILocationReadRepository _locationReadRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISequenceRepository _sequenceRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        public AddDoctorCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            ILocationReadRepository locationReadRepository,
            IUserRepository userRepository,
            ISequenceRepository sequenceRepository,
            IDoctorWriteRepository doctorWriteRepository,
            ISpecialtyReadRepository specialtyReadRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _userRepository = userRepository;
            _sequenceRepository = sequenceRepository;
            _locationReadRepository = locationReadRepository;
            _doctorWriteRepository = doctorWriteRepository;
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<string> Handle(AddDoctorCommand request, CancellationToken cancellationToken)
        {
            var specialties = await _specialtyReadRepository.GetAsync(cancellationToken: cancellationToken);

            await ValidateAndThrowAsync(request.Doctor, cancellationToken);

            var doctor = _mapper.Map<Doctor>(request.Doctor);

            doctor.DoctorSpecialties = new();

            foreach (var specialty in request.Doctor.Specialties)
            {
                var specialtyDb = specialties.First(x => x.Id + "" == specialty.Id);

                doctor.DoctorSpecialties.Add(new DoctorSpecialty
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    SpecialtyId = specialtyDb.Id,
                });
            }

            doctor.Pname = await _locationReadRepository.GetNameByIdAsync(doctor.Pid, "province", cancellationToken);
            doctor.Dname = await _locationReadRepository.GetNameByIdAsync(doctor.Did, "district", cancellationToken);
            doctor.Wname = await _locationReadRepository.GetNameByIdAsync(doctor.Wid, "ward", cancellationToken);

            await _doctorWriteRepository.AddDoctorAsync(doctor, cancellationToken: cancellationToken);

            await _sequenceRepository.IncreaseValueAsync(new Employee().GetTableName(), cancellationToken);

            //employee.AddDomainEvent(new AddEmployeeDomainEvent(employee));

            await _doctorWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return doctor.Id.ToString();
        }

        public async Task ValidateAndThrowAsync(DoctorDto doctor, CancellationToken cancellationToken)
        {
            var codeExist = await _userRepository.CodeExistAsync(doctor.Code, cancellationToken: cancellationToken);
            if (codeExist)
            {
                throw new BadRequestException(ErrorCode.CODE_EXISTED, _localizer["Account.CodeAlreadyExisted"]);
            }

            var phoneExist = await _userRepository.PhoneExistAsync(doctor.Phone, cancellationToken: cancellationToken);
            if (phoneExist)
            {
                throw new BadRequestException(_localizer["Account.PhoneAlreadyExists"]);
            }

            var emailExist = await _userRepository.EmailExistAsync(doctor.Email, cancellationToken: cancellationToken);
            if (emailExist)
            {
                throw new BadRequestException(_localizer["Account.EmailAlreadyExists"]);
            }
        }
    }
}
