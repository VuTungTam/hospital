using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Locations
{
    public class GetProvinceIdByNameQuery : BaseQuery<int>
    {
        public GetProvinceIdByNameQuery(string pName)
        {
            PName = pName;
        }
        public string PName { get;}
    }
}
