﻿using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.SharedKernel.Application.CQRS.Commands
{
    public class DeleteCommandHandler<T, TResponse, TWriteRepository> : BaseCommandHandler, IRequestHandler<DeleteCommand<T, TResponse>, TResponse>
        where T : BaseEntity
        where TWriteRepository : IWriteRepository<T>
    {
        private readonly IReadRepository<T> _readRepository;
        protected readonly IWriteRepository<T> _writeRepository;

        public DeleteCommandHandler(
            IStringLocalizer<Resources> localizer,
            IReadRepository<T> readRepository,
            TWriteRepository writeRepository
        ) : base(localizer)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public async Task<TResponse> Handle(DeleteCommand<T, TResponse> request, CancellationToken cancellationToken)
        {
            if (!request.IsValidIds())
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            await ValidateAndThrowAsync(request, cancellationToken);

            var entities = await _readRepository.GetByIdsAsync(request.Ids, cancellationToken: cancellationToken);
            if (entities.Any())
            {
                await _writeRepository.DeleteAsync(entities, cancellationToken);
            }

            if (typeof(TResponse) == typeof(Unit))
            {
                return (TResponse)Convert.ChangeType(Unit.Value, typeof(TResponse));
            }
            return (TResponse)Convert.ChangeType(request.Ids, typeof(TResponse));
        }

        protected virtual Task ValidateAndThrowAsync(DeleteCommand<T, TResponse> request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class DeleteCommandHandler<T, TResponse> : DeleteCommandHandler<T, TResponse, IWriteRepository<T>> where T : BaseEntity
    {
        public DeleteCommandHandler(IStringLocalizer<Resources> localizer, IReadRepository<T> readRepository, IWriteRepository<T> writeRepository) : base(localizer, readRepository, writeRepository)
        {
        }
    }
}