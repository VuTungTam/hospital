using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Helpers;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Doctors
{
    public class UpdateDoctorCommandHandler : BaseCommandHandler, IRequestHandler<UpdateDoctorCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly IDoctorWriteRepository _doctorWriteRepository;
        private readonly ILocationReadRepository _locationReadRepository;
        private readonly IAuthRepository _authRepository;

        public UpdateDoctorCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IUserRepository userRepository,
            IDoctorReadRepository doctorReadRepository,
            IDoctorWriteRepository doctorWriteRepository,
            ILocationReadRepository locationReadRepository,
            IAuthRepository authRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _userRepository = userRepository;
            _doctorReadRepository = doctorReadRepository;
            _doctorWriteRepository = doctorWriteRepository;
            _locationReadRepository = locationReadRepository;
            _authRepository = authRepository;
        }

        public async Task<Unit> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            request.Doctor.Phone = PhoneHelper.TransferToDomainPhone(request.Doctor.Phone);

            await ValidateAndThrowAsync(request.Doctor, cancellationToken);

            var option = new QueryOption
            {
                Includes = new string[] { nameof(Doctor.DoctorSpecialties) }
            };

            var doctor = await _doctorReadRepository.GetByIdAsync(long.Parse(request.Doctor.Id), option, cancellationToken: cancellationToken);
            if (doctor == null)
            {
                throw new BadRequestException("Nhân viên không tồn tại");
            }

            var newPname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Doctor.Pid), "province", cancellationToken); ;
            var newDname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Doctor.Did), "district", cancellationToken);
            var newWname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Doctor.Wid), "ward", cancellationToken);

            // var newDoctor = _mapper.Map<Doctor>(request.Doctor);
            doctor.Pname = newPname;
            doctor.Dname = newDname;
            doctor.Wname = newWname;

            doctor.DoctorTitle = request.Doctor.DoctorTitle;
            doctor.DoctorDegree = request.Doctor.DoctorDegree;
            doctor.DoctorRank = request.Doctor.DoctorRank;
            doctor.ExpertiseVn = request.Doctor.ExpertiseVn;
            doctor.ExpertiseEn = request.Doctor.ExpertiseEn;

            doctor.Status = request.Doctor.Status;
            doctor.Avatar = request.Doctor.Avatar;
            doctor.Name = request.Doctor.Name;
            doctor.Dob = request.Doctor.Dob;
            doctor.Phone = request.Doctor.Phone;
            doctor.Email = request.Doctor.Email;
            doctor.Pid = int.Parse(request.Doctor.Pid);
            doctor.Did = int.Parse(request.Doctor.Did);
            doctor.Wid = int.Parse(request.Doctor.Wid);
            doctor.Address = request.Doctor.Address;
            doctor.Pname = newPname;
            doctor.Dname = newDname;
            doctor.Wname = newWname;

            if (doctor.Status != AccountStatus.Active)
            {
                await _authRepository.RemoveRefreshTokensAsync(new List<long> { doctor.Id }, cancellationToken);
            }

            await _doctorWriteRepository.UpdateDoctorAsync(doctor, request.Doctor, cancellationToken: cancellationToken);

            // Nếu ko kích hoạt thì force logout, nếu recover thành kích hoạt thì xóa force logout
            if (doctor.Status != AccountStatus.Active)
            {
                await _authService.ForceLogoutAsync(doctor.Id, cancellationToken);
            }
            return Unit.Value;
        }

        private async Task ValidateAndThrowAsync(DoctorDto doctor, CancellationToken cancellationToken)
        {
            if (!long.TryParse(doctor.Id, out var id) || id <= 0)
            {
                throw new BadRequestException("ID không hợp lệ");
            }
            var codeExist = await _userRepository.CodeExistAsync(doctor.Code, id, cancellationToken);
            if (codeExist)
            {
                throw new BadRequestException(ErrorCode.CODE_EXISTED, "Mã bác sĩ đã tồn tại");
            }

            var phoneExist = await _userRepository.PhoneExistAsync(doctor.Phone, id, cancellationToken);
            if (phoneExist)
            {
                throw new BadRequestException("Số điện thoại đã tồn tại");
            }

            var emailExist = await _userRepository.EmailExistAsync(doctor.Email, id, cancellationToken);
            if (emailExist)
            {
                throw new BadRequestException("Email đã tồn tại");
            }
        }
    }
}
