using AutoMapper;
using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.SharedKernel.Application.CQRS.Commands
{
    public class AddCommandHandler<T, TDto, TResponse, TWriteRepository> : BaseCommandHandler, IRequestHandler<AddCommand<T, TDto, TResponse>, TResponse>
        where T : BaseEntity
        where TWriteRepository : IWriteRepository<T>
    {
        protected readonly IMapper _mapper;
        protected readonly IWriteRepository<T> _writeRepository;

        public AddCommandHandler(
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

        public virtual async Task<TResponse> Handle(AddCommand<T, TDto, TResponse> request, CancellationToken cancellationToken)
        {
            await ValidateAndThrowAsync(request, cancellationToken);

            var entity = _mapper.Map<T>(request.Dto);
            await _writeRepository.AddAsync(entity, cancellationToken);

            return (TResponse)Convert.ChangeType(entity.Id.ToString(), typeof(TResponse));
        }

        protected virtual Task ValidateAndThrowAsync(AddCommand<T, TDto, TResponse> request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
    public class AddCommandHandler<T, TDto, TResponse> : AddCommandHandler<T, TDto, TResponse, IWriteRepository<T>> where T : BaseEntity
    {
        public AddCommandHandler(IEventDispatcher eventDispatcher, IAuthService authService, IStringLocalizer<Resources> localizer, IMapper mapper, IWriteRepository<T> writeRepository) : base(eventDispatcher, authService, localizer, mapper, writeRepository)
        {
        }
    }
}
