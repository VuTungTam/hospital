using AutoMapper;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Application.Repositories.Interfaces.Employees;
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
    public class AddEmployeeCommandHandler : BaseCommandHandler, IRequestHandler<AddEmployeeCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly IRoleReadRepository _roleReadRepository;
        private readonly ISequenceRepository _sequenceRepository;
        private readonly ILocationReadRepository _locationReadRepository;

        public AddEmployeeCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IUserRepository userRepository,
            IEmployeeWriteRepository employeeWriteRepository,
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
        }

        public async Task<string> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var roles = await _roleReadRepository.GetAsync(cancellationToken: cancellationToken);

            await ValidateAndThrowAsync(request.Employee, roles, cancellationToken);

            var employee = _mapper.Map<Employee>(request.Employee);

            employee.EmployeeRoles = new();

            foreach (var role in request.Employee.Roles)
            {
                var roleDb = roles.First(x => x.Id + "" == role.Id);

                employee.EmployeeRoles.Add(new EmployeeRole
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    RoleId = roleDb.Id,
                });
            }

            employee.Pname = await _locationReadRepository.GetNameByIdAsync(employee.Pid, "province", cancellationToken);
            employee.Dname = await _locationReadRepository.GetNameByIdAsync(employee.Did, "district", cancellationToken);
            employee.Wname = await _locationReadRepository.GetNameByIdAsync(employee.Wid, "ward", cancellationToken);

            await _employeeWriteRepository.AddEmployeeAsync(employee, cancellationToken);

            await _sequenceRepository.IncreaseValueAsync(new Employee().GetTableName(), cancellationToken);

            //employee.AddDomainEvent(new AddEmployeeDomainEvent(employee));

            await _employeeWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            return employee.Id.ToString();
        }

        public async Task ValidateAndThrowAsync(EmployeeDto employee, List<Role> roles, CancellationToken cancellationToken)
        {
            var codeExist = await _userRepository.CodeExistAsync(employee.Code, cancellationToken: cancellationToken);
            if (codeExist)
            {
                throw new BadRequestException(ErrorCode.CODE_EXISTED, _localizer["Account.CodeAlreadyExisted"]);
            }

            var phoneExist = await _userRepository.PhoneExistAsync(employee.Phone, cancellationToken: cancellationToken);
            if (phoneExist)
            {
                throw new BadRequestException(_localizer["Account.PhoneAlreadyExists"]);
            }

            var emailExist = await _userRepository.EmailExistAsync(employee.Email, cancellationToken: cancellationToken);
            if (emailExist)
            {
                throw new BadRequestException(_localizer["Account.EmailAlreadyExists"]);
            }

            var sa = roles.First(x => x.Code == RoleCodeConstant.SUPER_ADMIN);
            if (employee.Roles.Any(x => x.Id == sa.Id + ""))
            {
                throw new BadRequestException(string.Format(_localizer["Roles.CannotExist"], sa.Name));
            }

            if (employee.Roles.Select(x => x.Id)
                          .Except(roles.Select(x => x.Id.ToString()))
                          .Any()
            )
            {
                throw new BadRequestException(_localizer["Roles.OneOrMoreRolesNotFound"]);
            }
        }
    }
}
