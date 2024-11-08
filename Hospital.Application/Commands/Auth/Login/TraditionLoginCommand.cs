using Hospital.Application.Dtos.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Models;

namespace Hospital.Application.Commands.Auth.Login
{
    public class TraditionLoginCommand : BaseAllowAnonymousCommand<LoginResult>
    {
        public TraditionLoginCommand(TraditionLoginDto dto, string with) 
        {
            Dto = dto;
            With = with;
        }
        public TraditionLoginDto Dto { get; }
        public string With { get; }
    }
}
