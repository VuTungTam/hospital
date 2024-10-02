using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Domain.Entities.Base;
using MediatR;

namespace Hospital.SharedKernel.Application.CQRS.Commands
{
    public class UpdateCommand<T, TDto, TResponse> : BaseCommand<TResponse> where T : BaseEntity
    {
        public UpdateCommand(TDto dto)
        {
            Dto = dto;
        }

        public TDto Dto { get; }
    }
    public class UpdateCommand<T, TDto> : UpdateCommand<T, TDto, Unit> where T : BaseEntity
    {
        public UpdateCommand(TDto dto) : base(dto)
        {
        }
    }
}
