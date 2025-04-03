using Hospital.Application.Dtos.Zones;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Zones
{
    public class GetZoneByIdQuery : BaseQuery<ZoneDto>
    {
        public GetZoneByIdQuery(long id) 
        {
            Id = id;
        }
        public long Id { get; }
    }
}
