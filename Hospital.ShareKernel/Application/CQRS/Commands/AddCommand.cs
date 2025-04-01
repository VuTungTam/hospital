using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.SharedKernel.Application.CQRS.Commands
{
    [RequiredPermission(ActionExponent.Add)]
    public class AddCommand<T, TDto, TResponse> : BaseCommand<TResponse> where T : BaseEntity
    {
        public AddCommand(TDto dto)
        {
            Dto = dto;
        }

        public TDto Dto { get; }
    }
    public class AddCommand<T, TDto> : AddCommand<T, TDto, string> where T : BaseEntity
    {
        public AddCommand(TDto dto) : base(dto)
        {
        }
    }

}
