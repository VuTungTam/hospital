using Hospital.Application.Dtos.Metas;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Metas
{
    [RequiredPermission(ActionExponent.Master)]
    public class UpdateScriptCommand : BaseCommand
    {
        public UpdateScriptCommand(ScriptDto script)
        {
            Script = script;
        }

        public ScriptDto Script { get; }
    }
}
