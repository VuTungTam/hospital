using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Locations
{
    public class GetWardIdByNameQuery : BaseQuery<int>
    {
        public GetWardIdByNameQuery(string wName, int did)
        {
            WName = wName;
            Did = did;
        }
        public string WName { get; }
        public int Did { get; }
    }
}
