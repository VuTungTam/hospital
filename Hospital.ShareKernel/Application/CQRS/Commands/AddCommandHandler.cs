using AutoMapper;
using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Repositories.Interface;
using Hospital.SharedKernel.Domain.Entities.Base;
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
        protected readonly IValidator<TDto> _validator;

        public AddCommandHandler(IValidator<TDto> validator, IStringLocalizer<Resources> localizer, IMapper mapper, IWriteRepository<T> writeRepository) : base(localizer)
        {
            this._validator = validator;
            this._mapper = mapper;
            this._writeRepository = writeRepository;
        }

        public AddCommandHandler(
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            TWriteRepository writeRepository,
            IValidator<TDto> validator
        ) : base(localizer)
        {
            _mapper = mapper;
            _writeRepository = writeRepository;
            _validator = validator;
        }

        public virtual async Task<TResponse> Handle(AddCommand<T, TDto, TResponse> request, CancellationToken cancellationToken)
        {
            //await ValidateAndThrowAsync(request, cancellationToken);

            var entity = _mapper.Map<T>(request.Dto);
            await _writeRepository.AddAsync(entity, cancellationToken);

            return (TResponse)Convert.ChangeType(entity.Id.ToString(), typeof(TResponse));
        }

        protected async virtual Task ValidateAndThrowAsync(AddCommand<T, TDto, TResponse> request, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<AddCommand<T, TDto, TResponse>>(request);
            var result = await _validator.ValidateAsync(context, cancellationToken);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
    public class AddCommandHandler<T, TDto, TResponse> : AddCommandHandler<T, TDto, TResponse, IWriteRepository<T>> where T : BaseEntity
    {
        public AddCommandHandler(IValidator<TDto> validator, IStringLocalizer<Resources> localizer, IMapper mapper, IWriteRepository<T> writeRepository) : base(validator, localizer, mapper, writeRepository)
        {
        }
    }
}
