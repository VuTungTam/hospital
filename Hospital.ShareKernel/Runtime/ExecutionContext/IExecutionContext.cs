using Hospital.SharedKernel.Application.Enums;
using Microsoft.AspNetCore.Http;

namespace Hospital.SharedKernel.Runtime.ExecutionContext
{
    public interface IExecutionContext
    {
        string TraceId { get; }

        string AccessToken { get; }

        string Uid { get; }

        string Email { get; }

        string Permission { get; }

        long Identity { get; }

        long FacilityId { get; }

        long ZoneId { get; }

        bool IsAnonymous { get; }

        bool IsSA { get; }

        AccountType AccountType { get; }

        HttpContext HttpContext { get; }

        void UpdateContext(string accessToken);

        void MakeAnonymousRequest();
    }
}
