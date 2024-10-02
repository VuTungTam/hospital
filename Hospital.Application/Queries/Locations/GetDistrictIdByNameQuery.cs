using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Infrastructure.Repositories.Locations.Interfaces;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Locations
{
    public class GetDistrictIdByNameQuery : BaseQuery<int>
    {
        public GetDistrictIdByNameQuery(string dName, int pid)
        {
            DName = dName;
            Pid = pid;
        }
        public string DName { get; }
        public int Pid { get; }
    }
}
