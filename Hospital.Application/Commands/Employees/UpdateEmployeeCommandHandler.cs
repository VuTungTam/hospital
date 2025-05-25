using AutoMapper;
using Hospital.Application.Dtos.Employee;
using Hospital.Application.Helpers;
using Hospital.Application.Repositories.Interfaces.Auth;
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
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Employees
{
    public class UpdateEmployeeCommandHandler : BaseCommandHandler, IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly IRoleReadRepository _roleReadRepository;
        private readonly ILocationReadRepository _locationReadRepository;
        private readonly IAuthRepository _authRepository;

        public UpdateEmployeeCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IUserRepository userRepository,
            IEmployeeReadRepository employeeReadRepository,
            IEmployeeWriteRepository employeeWriteRepository,
            IRoleReadRepository roleReadRepository,
            ILocationReadRepository locationReadRepository,
            IAuthRepository authRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _userRepository = userRepository;
            _employeeReadRepository = employeeReadRepository;
            _employeeWriteRepository = employeeWriteRepository;
            _roleReadRepository = roleReadRepository;
            _locationReadRepository = locationReadRepository;
            _authRepository = authRepository;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var roles = await _roleReadRepository.GetAsync(cancellationToken: cancellationToken);

            request.Employee.Phone = PhoneHelper.TransferToDomainPhone(request.Employee.Phone);

            await ValidateAndThrowAsync(request.Employee, roles, cancellationToken);

            var employee = await _employeeReadRepository.GetByIdIncludedRolesAsync(long.Parse(request.Employee.Id), cancellationToken);
            if (employee == null)
            {
                throw new BadRequestException("Nhân viên không tồn tại");
            }

            var oldRoles = employee.EmployeeRoles.Select(x => x.Role).ToList();
            var newRoles = roles.Where(x => request.Employee.Roles.Select(x => long.Parse(x.Id)).Contains(x.Id)).ToList();
            var isSuperAdmin = oldRoles.Any(x => x.Code == RoleCodeConstant.SUPER_ADMIN);
            if (isSuperAdmin && _executionContext.Identity != employee.Id)
            {
                throw new ForbiddenException("Không được quyền cập nhật tài khoản quản trị");
            }

            if (isSuperAdmin && _executionContext.Identity == employee.Id && request.Employee.Status != AccountStatus.Active)
            {
                throw new BadRequestException("Không thể chuyển trạng thái tài khoản quản trị");
            }

            if (!isSuperAdmin)
            {
                var sa = roles.First(x => x.Code == RoleCodeConstant.SUPER_ADMIN);
                if (newRoles.Any(x => x.Id == sa.Id))
                {
                    throw new BadRequestException($"Không thể có vai trò là {sa.Name}");
                }
            }

            var newPname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Employee.Pid), "province", cancellationToken); ;
            var newDname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Employee.Did), "district", cancellationToken);
            var newWname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Employee.Wid), "ward", cancellationToken);

            var newEmployee = _mapper.Map<Employee>(request.Employee);
            newEmployee.Pname = newPname;
            newEmployee.Dname = newDname;
            newEmployee.Wname = newWname;
            var oldState = employee.Status;

            employee.EmployeeRoles = new();
            employee.Status = request.Employee.Status;
            employee.Avatar = request.Employee.Avatar;
            employee.Name = request.Employee.Name;
            employee.Dob = request.Employee.Dob;
            employee.Phone = request.Employee.Phone;
            employee.Email = request.Employee.Email;
            employee.Pid = int.Parse(request.Employee.Pid);
            employee.Did = int.Parse(request.Employee.Did);
            employee.Wid = int.Parse(request.Employee.Wid);
            employee.ZoneId = long.Parse(request.Employee.ZoneId);
            employee.Address = request.Employee.Address;
            employee.Pname = newPname;
            employee.Dname = newDname;
            employee.Wname = newWname;

            if (!isSuperAdmin && employee.Id != _executionContext.Identity)
            {
                await _employeeWriteRepository.UpdateRolesAsync(employee.Id, request.Employee.Roles.Select(x => long.Parse(x.Id)), cancellationToken);
            }

            if (employee.Status != AccountStatus.Active)
            {
                await _authRepository.RemoveRefreshTokensAsync(new List<long> { employee.Id }, cancellationToken);
            }

            await _employeeWriteRepository.UpdateAsync(employee, cancellationToken: cancellationToken);

            // Nếu ko kích hoạt thì force logout, nếu recover thành kích hoạt thì xóa force logout
            if (employee.Status != AccountStatus.Active)
            {
                await _authService.ForceLogoutAsync(employee.Id, cancellationToken);
            }
            return Unit.Value;
        }

        private async Task ValidateAndThrowAsync(EmployeeDto employee, List<Role> roles, CancellationToken cancellationToken)
        {
            if (!long.TryParse(employee.Id, out var id) || id <= 0)
            {
                throw new BadRequestException("ID không hợp lệ");
            }

            var codeExist = await _userRepository.CodeExistAsync(employee.Code, id, cancellationToken);
            if (codeExist)
            {
                throw new BadRequestException(ErrorCode.CODE_EXISTED, "Mã nhân viên đã tồn tại");
            }

            var phoneExist = await _userRepository.PhoneExistAsync(employee.Phone, id, cancellationToken);
            if (phoneExist)
            {
                throw new BadRequestException("Số điện thoại đã tồn tại");
            }

            var emailExist = await _userRepository.EmailExistAsync(employee.Email, id, cancellationToken);
            if (emailExist)
            {
                throw new BadRequestException("Email đã tồn tại");
            }

            if (employee.Roles.Select(x => x.Id)
                          .Except(roles.Select(x => x.Id.ToString()))
                          .Any()
            )
            {
                throw new BadRequestException("Tồn tại vai trò không có trong hệ thống. Vui lòng kiểm tra lại");
            }
        }

        //private string GetAuditDetail(Employee employee, Employee newEmployee, List<Role> oldRoles, List<Role> newRoles)
        //{
        //    var detail = new StringBuilder();

        //    if (employee.Status != newEmployee.Status)
        //    {
        //        detail.AppendLine($"<p>Trạng thái: <strong>{employee.Status.GetDescription()}</strong> => <strong>{newEmployee.Status.GetDescription()}</strong></p>");
        //    }

        //    if (employee.Avatar != newEmployee.Avatar)
        //    {
        //        if (string.IsNullOrEmpty(employee.Avatar) && !string.IsNullOrEmpty(newEmployee.Avatar))
        //        {
        //            detail.AppendLine($"<p>Thêm ảnh đại điện: <a href='{CdnConfig.Get(newEmployee.Avatar)}' target='_blank''>Chi tiết</a></p>");
        //        }
        //        else if (!string.IsNullOrEmpty(employee.Avatar) && string.IsNullOrEmpty(newEmployee.Avatar))
        //        {
        //            detail.AppendLine($"<p>Xóa ảnh đại điện: <a href='{CdnConfig.Get(employee.Avatar)}' target='_blank''>Chi tiết</a></p>");
        //        }
        //        else
        //        {
        //            detail.AppendLine($"<p>Ảnh đại điện: <a href='{CdnConfig.Get(employee.Avatar)}' target='_blank''>Ảnh cũ</a> => <a href='{CdnConfig.Get(newEmployee.Avatar)}' target='_blank''>Ảnh mới</a></p>");
        //        }
        //    }

        //    if (employee.Name != newEmployee.Name)
        //    {
        //        detail.AppendLine($"<p>Họ và tên: <strong>{employee.Name}</strong> => <strong>{newEmployee.Name}</strong></p>");
        //    }

        //    if (employee.Dob != newEmployee.Dob)
        //    {
        //        detail.AppendLine($"<p>Ngày sinh: <strong>{employee.Dob:dd/MM/yyyy}</strong> => <strong>{newEmployee.Dob:dd/MM/yyyy}</strong></p>");
        //    }

        //    if (PhoneHelper.TransferToDomainPhone(employee.Phone) != PhoneHelper.TransferToDomainPhone(newEmployee.Phone))
        //    {
        //        detail.AppendLine($"<p>Số điện thoại: <strong>{PhoneHelper.TransferToDomainPhone(employee.Phone)}</strong> => <strong>{PhoneHelper.TransferToDomainPhone(newEmployee.Phone)}</strong></p>");
        //    }

        //    if (employee.Email != newEmployee.Email)
        //    {
        //        detail.AppendLine($"<p>Email: <strong>{employee.Email}</strong> => <strong>{newEmployee.Email}</strong></p>");
        //    }

        //    if (employee.Pid != newEmployee.Pid)
        //    {
        //        detail.AppendLine($"<p>Tỉnh/thành: <strong>{employee.Pname}</strong> => <strong>{newEmployee.Pname}</strong></p>");
        //    }

        //    if (employee.Did != newEmployee.Did)
        //    {
        //        detail.AppendLine($"<p>Quận/huyện: <strong>{employee.Dname}</strong> => <strong>{newEmployee.Dname}</strong></p>");
        //    }

        //    if (employee.Wid != newEmployee.Wid)
        //    {
        //        detail.AppendLine($"<p>Xã/phường: <strong>{employee.Wname}</strong> => <strong>{newEmployee.Wname}</strong></p>");
        //    }

        //    if (employee.Address != newEmployee.Address)
        //    {
        //        detail.AppendLine($"<p>Địa chỉ: <strong>{employee.Address}</strong> => <strong>{newEmployee.Address}</strong></p>");
        //    }

        //    var otherRoles = newRoles.Select(x => x.Id).Except(oldRoles.Select(x => x.Id));
        //    if (otherRoles.Any())
        //    {
        //        var oldRoleNames = string.Join(", ", oldRoles.Select(x => x.Name));
        //        var newRoleNames = string.Join(", ", newRoles.Select(x => x.Name));

        //        detail.AppendLine($"<p>Vai trò: <strong>{oldRoleNames}</strong> => <strong>{newRoleNames}</strong></p>");
        //    }

        //    return detail.ToString();
        //}
    }
}
