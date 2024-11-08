using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Sequences
{
    [RequiredPermission(ActionExponent.View)]
    public class GetSequenceQuery : BaseQuery<string>
    {
        public GetSequenceQuery(string table)
        {
            Table = table;
        }

        public string Table { get; }
    }
}
