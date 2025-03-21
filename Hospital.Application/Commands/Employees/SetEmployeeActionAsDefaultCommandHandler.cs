using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Employees
{
    public class SetEmployeeActionAsDefaultCommandHandler : BaseCommandHandler, IRequestHandler<SetEmployeeActionAsDefaultCommand>
    {
        private readonly IEmployeeWriteRepository _employeeWriteRepository;

        public SetEmployeeActionAsDefaultCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IEmployeeWriteRepository employeeWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _employeeWriteRepository = employeeWriteRepository;
        }

        public async Task<Unit> Handle(SetEmployeeActionAsDefaultCommand request, CancellationToken cancellationToken)
        {
            if (request.EmployeeId <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            await _employeeWriteRepository.SetActionAsDefaultAsync(request.EmployeeId, cancellationToken);
            await _authService.FetchNewTokenAsync(request.EmployeeId, "Quyền của bạn vừa được thay đổi. Tải lại?", cancellationToken);

            return Unit.Value;
        }
    }
}
