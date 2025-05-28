using AutoMapper;
using Hospital.Application.Commands.HealthServices;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Queries.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthServices
{
    public class HealthServiceController : ApiBaseController
    {
        public HealthServiceController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("filterable")]
        public IActionResult GetFilterable() => base.GetFilterable<HealthService>();

        [HttpGet("enums"), AllowAnonymous]
        public IActionResult GetEnums(string noneOption, bool vn) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<HealthServiceStatus>(noneOption, vn) });

        [HttpGet("type"), AllowAnonymous]
        public async Task<IActionResult> GetAllServiceType(CancellationToken cancellationToken = default)
        {
            var query = new GetServiceTypeQuery();
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("facility/{facilityId}/type"), AllowAnonymous]
        public async Task<IActionResult> GetServiceTypeByFacilityId(long facilityId, CancellationToken cancellationToken = default)
        {
            var query = new GetServiceTypeByFacilityIdQuery(facilityId);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("type/slug/{slug}"), AllowAnonymous]
        public async Task<IActionResult> GetTypeBySlug(string slug, [FromQuery] List<string> langs, CancellationToken cancellationToken = default)
        {
            if (langs == null || !langs.Any())
            {
                langs = new List<string>
                {
                    "vi-VN",
                    "en-US"
                };
            }

            var query = new GetServiceTypeBySlugQuery(slug, langs);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet, AllowAnonymous]
        public virtual async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long typeId = 0,
            long facilityId = 0,
            long specialtyId = 0,
            long doctorId = 0,
            HealthServiceStatus status = HealthServiceStatus.None,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);

            var query = new GetHealthServicePagingQuery(pagination, facilityId, typeId, specialtyId, doctorId, status);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("{id}"), AllowAnonymous]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetHealthServiceByIdQuery(id);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }
        [HttpGet("current/pagination"), AllowAnonymous]
        public virtual async Task<IActionResult> GetCurrentServicePagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long doctorId = 0,
            long facilityId = 0,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);

            var query = new GetHealthServiceCurrentQuery(pagination, facilityId, doctorId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("current/{doctorId}"), AllowAnonymous]
        public virtual async Task<IActionResult> GetDoctorCurrentService(
            long doctorId = 0,
            CancellationToken cancellationToken = default)
        {
            var query = new GetDoctorContextQuery(doctorId);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(HealthServiceDto healthService, CancellationToken cancellationToken = default)
        {
            var command = new AddHealthServiceCommand(healthService);

            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]

        public virtual async Task<IActionResult> Update(HealthServiceDto healthService, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateHealthServiceCommand(healthService), cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteHealthServiceCommand(ids), cancellationToken);

            return Ok(new BaseResponse());
        }
    }
}
