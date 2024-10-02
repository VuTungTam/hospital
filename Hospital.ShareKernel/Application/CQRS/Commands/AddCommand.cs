using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Domain.Entities.Base;

namespace Hospital.SharedKernel.Application.CQRS.Commands
{
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
