using Hospital.Application.Models.Dashboards.Customers;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Dashboards.Customers
{
    [RequiredPermission(ActionExponent.ViewDashboard)]
    public class GetCustomerTrendQuery : BaseQuery<CustomerTrend>
    {
    }
}
