using Hospital.Application.Dtos.Metas;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Metas
{
    [RequiredPermission(ActionExponent.Master)]
    public class UpsertMetaTagCommand : BaseCommand
    {
        public UpsertMetaTagCommand(MetaDto meta)
        {
            Meta = meta;
        }

        public MetaDto Meta { get; }
    }
}
