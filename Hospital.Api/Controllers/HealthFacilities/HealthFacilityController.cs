using System;
using System.Threading;
using AutoMapper;
using Hospital.Application.Commands.HealthFacilities;
using Hospital.Application.Commands.Specialties;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Queries.FacilityTypes;
using Hospital.Application.Queries.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthFacilities
{
    public class HealthFacilityController : ApiBaseController
    {
        public HealthFacilityController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("entire")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var query = new GetAllFacilityQuery();
            var data = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = data, Total = data.Count });
        }

        [HttpGet("filterable")]
        public IActionResult GetFilterable() => GetFilterable<HealthFacility>();


        [HttpGet("enums"), AllowAnonymous]
        public IActionResult GetEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<HealthServiceStatus>(noneOption) });

        [HttpGet("type"), AllowAnonymous]
        public async Task<IActionResult> GetAllFacilityType(CancellationToken cancellationToken = default)
        {
            var query = new GetFacilityTypeQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long typeId = 0,
            long serviceTypeId = 0,
            HealthFacilityStatus status = HealthFacilityStatus.None,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetHealthFacilityPaginationQuery(pagination, typeId, serviceTypeId, status);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("most"), AllowAnonymous]
        public async Task<IActionResult> GetMostFacility(CancellationToken cancellationToken = default)
        {
            var query = new GetMostFacilityQuery();
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetFacilityByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpGet("slug/{slug}"), AllowAnonymous]
        public async Task<IActionResult> GetBySlug(string slug, [FromQuery] List<string> langs, CancellationToken cancellationToken = default)
        {
            if (langs == null || !langs.Any())
            {
                langs = new List<string>
                {
                    "vi-VN",
                    "en-US"
                };
            }

            var query = new GetFacilityBySlugQuery(slug, langs);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(query, cancellationToken) });
        }

        [HttpGet("name"), AllowAnonymous]
        public async Task<IActionResult> GetByName(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            CancellationToken cancellationToken = default
            )
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);

            var query = new GetFacilityNamePaginationQuery(pagination);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add(HealthFacilityDto healthFacility, CancellationToken cancellationToken = default)
        {
            var command = new AddHealthFacilityCommand(healthFacility);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(HealthFacilityDto healthFacility, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateHealthFacilityCommand(healthFacility), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteHealthFacilityCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("add-specialty/{facilityId}/{specialtyId}")]
        public async Task<IActionResult> AddAction(long facilityId, long specialtyId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new AddSpecialtyForFacilityCommand(facilityId, specialtyId), cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpDelete("remove-specialty/{facilityId}/{specialtyId}")]
        public async Task<IActionResult> RemoveAction(long facilityId, long specialtyId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new RemoveSpecialtyForFacilityCommand(facilityId, specialtyId), cancellationToken);
            return Ok(new BaseResponse());
        }
    }
}
