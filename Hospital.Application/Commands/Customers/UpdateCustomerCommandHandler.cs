using AutoMapper;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Helpers;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Users;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Customers;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Customers
{
    public class UpdateCustomerCommandHandler : BaseCommandHandler, IRequestHandler<UpdateCustomerCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly ILocationReadRepository _locationReadRepository;
        //private readonly IAuditWriteRepository _auditWriteRepository;
        private readonly IAuthRepository _authRepository;

        public UpdateCustomerCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IUserRepository userRepository,
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            ILocationReadRepository locationReadRepository,
            //IAuditWriteRepository auditWriteRepository,
            IAuthRepository authRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _userRepository = userRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _locationReadRepository = locationReadRepository;
            //_auditWriteRepository = auditWriteRepository;
            _authRepository = authRepository;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            request.Customer.Phone = PhoneHelper.TransferToDomainPhone(request.Customer.Phone);

            await ValidateAndThrowAsync(request.Customer, cancellationToken);

            var customer = await _customerReadRepository.GetByIdAsync(long.Parse(request.Customer.Id), cancellationToken: cancellationToken);
            if (customer == null)
            {
                throw new BadRequestException("Khách hàng không tồn tại");
            }

            var newPname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Customer.Pid), "province", cancellationToken); ;
            var newDname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Customer.Did), "district", cancellationToken);
            var newWname = await _locationReadRepository.GetNameByIdAsync(int.Parse(request.Customer.Wid), "ward", cancellationToken);

            var newCustomer = _mapper.Map<Customer>(request.Customer);
            newCustomer.Pname = newPname;
            newCustomer.Dname = newDname;
            newCustomer.Wname = newWname;
            //var auditDetail = GetAuditDetail(customer, newCustomer);
            var oldState = customer.Status;

            customer.Status = request.Customer.Status;
            customer.Avatar = request.Customer.Avatar;
            customer.Name = request.Customer.Name;
            customer.Dob = request.Customer.Dob;
            customer.Phone = request.Customer.Phone;
            customer.Email = request.Customer.Email;
            customer.Pid = int.Parse(request.Customer.Pid);
            customer.Did = int.Parse(request.Customer.Did);
            customer.Wid = int.Parse(request.Customer.Wid);
            customer.Address = request.Customer.Address;
            customer.Pname = newPname;
            customer.Dname = newDname;
            customer.Wname = newWname;

            //_auditWriteRepository.Add(new Audit
            //{
            //    Type = AuditType.Account,
            //    Category = typeof(BaseUser).Name.ToSnakeCaseLower(),
            //    Description = $"<p>Cập nhật thông tin khách hàng <strong>{customer.Name} ({customer.Code})</strong>:</p> {auditDetail}",
            //    IsSystemAction = true
            //});

            if (customer.Status != AccountStatus.Active)
            {
                await _authRepository.RemoveRefreshTokensAsync(new List<long> { customer.Id }, cancellationToken);
            }

            await _customerWriteRepository.UpdateAsync(customer, cancellationToken: cancellationToken);

            // Nếu ko kích hoạt thì force logout, nếu recover thành kích hoạt thì xóa force logout
            if (customer.Status != AccountStatus.Active)
            {
                await _authService.ForceLogoutAsync(customer.Id, cancellationToken);
            }
            return Unit.Value;
        }

        private async Task ValidateAndThrowAsync(CustomerDto customer, CancellationToken cancellationToken)
        {
            if (!long.TryParse(customer.Id, out var id) || id <= 0)
            {
                throw new BadRequestException("ID không hợp lệ");
            }

            var codeExist = await _userRepository.CodeExistAsync(customer.Code, id, cancellationToken);
            if (codeExist)
            {
                throw new BadRequestException(ErrorCode.CODE_EXISTED, "Mã khách hàng đã tồn tại");
            }

            var phoneExist = await _userRepository.PhoneExistAsync(customer.Phone, id, cancellationToken);
            if (phoneExist)
            {
                throw new BadRequestException("Số điện thoại đã tồn tại");
            }

            var emailExist = await _userRepository.EmailExistAsync(customer.Email, id, cancellationToken);
            if (emailExist)
            {
                throw new BadRequestException("Email đã tồn tại");
            }
        }

        //private string GetAuditDetail(Customer customer, Customer newCustomer)
        //{
        //    var detail = new StringBuilder();

        //    if (customer.Status != newCustomer.Status)
        //    {
        //        detail.AppendLine($"<p>Trạng thái: <strong>{customer.Status.GetDescription()}</strong> => <strong>{newCustomer.Status.GetDescription()}</strong></p>");
        //    }

        //    if (customer.Avatar != newCustomer.Avatar)
        //    {
        //        if (string.IsNullOrEmpty(customer.Avatar) && !string.IsNullOrEmpty(newCustomer.Avatar))
        //        {
        //            detail.AppendLine($"<p>Thêm ảnh đại điện: <a href='{CdnConfig.Get(newCustomer.Avatar)}' target='_blank''>Chi tiết</a></p>");
        //        }
        //        else if (!string.IsNullOrEmpty(customer.Avatar) && string.IsNullOrEmpty(newCustomer.Avatar))
        //        {
        //            detail.AppendLine($"<p>Xóa ảnh đại điện: <a href='{CdnConfig.Get(customer.Avatar)}' target='_blank''>Chi tiết</a></p>");
        //        }
        //        else
        //        {
        //            detail.AppendLine($"<p>Ảnh đại điện: <a href='{CdnConfig.Get(customer.Avatar)}' target='_blank''>Ảnh cũ</a> => <a href='{CdnConfig.Get(newCustomer.Avatar)}' target='_blank''>Ảnh mới</a></p>");
        //        }
        //    }

        //    if (customer.Name != newCustomer.Name)
        //    {
        //        detail.AppendLine($"<p>Họ và tên: <strong>{customer.Name}</strong> => <strong>{newCustomer.Name}</strong></p>");
        //    }

        //    if (customer.Dob != newCustomer.Dob)
        //    {
        //        detail.AppendLine($"<p>Ngày sinh: <strong>{customer.Dob:dd/MM/yyyy}</strong> => <strong>{newCustomer.Dob:dd/MM/yyyy}</strong></p>");
        //    }

        //    if (PhoneHelper.TransferToDomainPhone(customer.Phone) != PhoneHelper.TransferToDomainPhone(newCustomer.Phone))
        //    {
        //        detail.AppendLine($"<p>Số điện thoại: <strong>{PhoneHelper.TransferToDomainPhone(customer.Phone)}</strong> => <strong>{PhoneHelper.TransferToDomainPhone(newCustomer.Phone)}</strong></p>");
        //    }

        //    if (customer.Email != newCustomer.Email)
        //    {
        //        detail.AppendLine($"<p>Email: <strong>{customer.Email}</strong> => <strong>{newCustomer.Email}</strong></p>");
        //    }

        //    if (customer.Pid != newCustomer.Pid)
        //    {
        //        detail.AppendLine($"<p>Tỉnh/thành: <strong>{customer.Pname}</strong> => <strong>{newCustomer.Pname}</strong></p>");
        //    }

        //    if (customer.Did != newCustomer.Did)
        //    {
        //        detail.AppendLine($"<p>Quận/huyện: <strong>{customer.Dname}</strong> => <strong>{newCustomer.Dname}</strong></p>");
        //    }

        //    if (customer.Wid != newCustomer.Wid)
        //    {
        //        detail.AppendLine($"<p>Xã/phường: <strong>{customer.Wname}</strong> => <strong>{newCustomer.Wname}</strong></p>");
        //    }

        //    if (customer.Address != newCustomer.Address)
        //    {
        //        detail.AppendLine($"<p>Địa chỉ: <strong>{customer.Address}</strong> => <strong>{newCustomer.Address}</strong></p>");
        //    }

        //    return detail.ToString();
        //}
    }
}
