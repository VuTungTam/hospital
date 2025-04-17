using AutoMapper;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Employees
{
    public class AddFacilityAdminCommandHandler : BaseCommandHandler, IRequestHandler<AddFacilityAdminCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly IRoleReadRepository _roleReadRepository;
        private readonly ISequenceRepository _sequenceRepository;
        private readonly ILocationReadRepository _locationReadRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        public AddFacilityAdminCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IUserRepository userRepository,
            IEmployeeWriteRepository employeeWriteRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            IRoleReadRepository roleReadRepository,
            ISequenceRepository sequenceRepository,
            ILocationReadRepository locationReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _userRepository = userRepository;
            _employeeWriteRepository = employeeWriteRepository;
            _roleReadRepository = roleReadRepository;
            _sequenceRepository = sequenceRepository;
            _locationReadRepository = locationReadRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<string> Handle(AddFacilityAdminCommand request, CancellationToken cancellationToken)
        {
            var facility = await _healthFacilityReadRepository.GetByIdAsync(long.Parse(request.Admin.FacilityId), cancellationToken: cancellationToken);

            if (facility == null) {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var roles = await _roleReadRepository.GetAsync(cancellationToken: cancellationToken);

            var roleFacilityAdmin = roles.FirstOrDefault(x => x.Code == RoleCodeConstant.FACILITY_ADMIN);

            await ValidateAndThrowAsync(request.Admin, roles, cancellationToken);

            var facilityAdmin = _mapper.Map<Employee>(request.Admin);

            facilityAdmin.EmployeeRoles = new();

            facilityAdmin.EmployeeRoles.Add(new EmployeeRole
            {
                Id = AuthUtility.GenerateSnowflakeId(),
                RoleId = roleFacilityAdmin.Id,
            });

            facilityAdmin.Pname = await _locationReadRepository.GetNameByIdAsync(facilityAdmin.Pid, "province", cancellationToken);
            facilityAdmin.Dname = await _locationReadRepository.GetNameByIdAsync(facilityAdmin.Did, "district", cancellationToken);
            facilityAdmin.Wname = await _locationReadRepository.GetNameByIdAsync(facilityAdmin.Wid, "ward", cancellationToken);

            await _employeeWriteRepository.AddFacilityAdminAsync(facilityAdmin, facilityAdmin.FacilityId, cancellationToken);

            await _sequenceRepository.IncreaseValueAsync("admin", cancellationToken);

            //employee.AddDomainEvent(new AddEmployeeDomainEvent(employee));

            await _employeeWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return facilityAdmin.Id.ToString();
        }

        public async Task ValidateAndThrowAsync(AdminDto admin, List<Role> roles, CancellationToken cancellationToken)
        {
            var codeExist = await _userRepository.CodeExistAsync(admin.Code, cancellationToken: cancellationToken);
            if (codeExist)
            {
                throw new BadRequestException(ErrorCode.CODE_EXISTED, _localizer["Account.CodeAlreadyExisted"]);
            }

            var phoneExist = await _userRepository.PhoneExistAsync(admin.Phone, cancellationToken: cancellationToken);
            if (phoneExist)
            {
                throw new BadRequestException(_localizer["Account.PhoneAlreadyExists"]);
            }

            var emailExist = await _userRepository.EmailExistAsync(admin.Email, cancellationToken: cancellationToken);
            if (emailExist)
            {
                throw new BadRequestException(_localizer["Account.EmailAlreadyExists"]);
            }
        }
    }
}
