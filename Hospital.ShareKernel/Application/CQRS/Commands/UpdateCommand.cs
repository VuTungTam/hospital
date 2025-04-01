using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Libraries.Attributes;
using MediatR;

namespace Hospital.SharedKernel.Application.CQRS.Commands
{
    [RequiredPermission(ActionExponent.Update)]
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
