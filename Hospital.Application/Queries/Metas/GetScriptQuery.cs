using Hospital.Application.Dtos.Metas;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Metas
{
    public class GetScriptQuery : BaseAllowAnonymousQuery<ScriptDto>
    {
    }
}
