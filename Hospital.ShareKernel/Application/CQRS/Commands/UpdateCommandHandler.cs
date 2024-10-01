using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.SharedKernel.Application.CQRS.Commands
{
    public class UpdateCommandHandler<T, TDto, TResponse, TWriteRepository> : BaseCommandHandler, IRequestHandler<UpdateCommand<T, TDto, TResponse>, TResponse>
        where T : BaseEntity
        where TWriteRepository : IWriteRepository<T>
    {
        protected readonly IMapper _mapper;
        protected readonly IWriteRepository<T> _writeRepository;

        public UpdateCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            TWriteRepository writeRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _writeRepository = writeRepository;
        }

        public async Task<TResponse> Handle(UpdateCommand<T, TDto, TResponse> request, CancellationToken cancellationToken)
        {
            await ValidateAndThrowAsync(request, cancellationToken);

            var entity = _mapper.Map<T>(request.Dto);
            if (entity.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            await _writeRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            if (typeof(TResponse) == typeof(Unit))
            {
                return (TResponse)Convert.ChangeType(Unit.Value, typeof(TResponse));
            }
            return (TResponse)Convert.ChangeType(entity.Id.ToString(), typeof(TResponse));
        }

        protected virtual Task ValidateAndThrowAsync(UpdateCommand<T, TDto, TResponse> request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class UpdateCommandHandler<T, TDto, TResponse> : UpdateCommandHandler<T, TDto, TResponse, IWriteRepository<T>> where T : BaseEntity
    {
        public UpdateCommandHandler(IEventDispatcher eventDispatcher, IAuthService authService, IStringLocalizer<Resources> localizer, IMapper mapper, IWriteRepository<T> writeRepository) : base(eventDispatcher, authService, localizer, mapper, writeRepository)
        {
        }
    }
}
