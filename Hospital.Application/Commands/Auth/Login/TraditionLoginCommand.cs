using Hospital.Application.Models.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Models;

namespace Hospital.Application.Commands.Auth.Login
{
    public class TraditionLoginCommand : BaseAllowAnonymousCommand<LoginResult>
    {
        public TraditionLoginCommand(TraditionLoginRequest dto) 
        {
            Dto = dto;
        }
        public TraditionLoginRequest Dto { get; }
    }
}
