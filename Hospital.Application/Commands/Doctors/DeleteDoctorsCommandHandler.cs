using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Doctors
{
    public class DeleteDoctorsCommandHandler : BaseCommandHandler, IRequestHandler<DeleteDoctorsCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly IDoctorWriteRepository _doctorWriteRepository;
        private readonly IAuthRepository _authRepository;

        public DeleteDoctorsCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IDoctorReadRepository doctorReadRepository,
            IDoctorWriteRepository doctorWriteRepository,
            IAuthRepository authRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _doctorReadRepository = doctorReadRepository;
            _doctorWriteRepository = doctorWriteRepository;
            _authRepository = authRepository;
        }

        public async Task<Unit> Handle(DeleteDoctorsCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
            {
                return Unit.Value;
            }
            if (request.Ids.Exists(x => x == _executionContext.Identity))
            {
                throw new BadRequestException("Không thể tự xóa tài khoản");
            }

            var doctors = await _doctorReadRepository.GetByIdsAsync(request.Ids, cancellationToken: cancellationToken);
            if (!doctors.Any())
            {
                return Unit.Value;
            }
            await _authRepository.RemoveRefreshTokensAsync(doctors.Select(x => x.Id), cancellationToken);

            await _doctorWriteRepository.DeleteAsync(doctors, cancellationToken);

            //await _eventDispatcher.FireEventAsync(new DeletedoctorsDomainEvent(doctors), cancellationToken);

            var forceLogoutTasks = doctors.Select(x => _authService.ForceLogoutAsync(x.Id, cancellationToken));

            await Task.WhenAll(forceLogoutTasks);

            return Unit.Value;
        }
    }
}
