﻿using Hospital.Application.Commands.Specialties;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Queries.Specialties;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Specialties
{
    public class SpecialtyController : ApiBaseController
    {
        public SpecialtyController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long facilityId = 0,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetSpecialtyPaginationQuery(pagination, facilityId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetSpecialtyByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpGet("doctor/{doctorId}"), AllowAnonymous]
        public async Task<IActionResult> GetByDoctorId(long doctorId, CancellationToken cancellationToken = default)
        {
            var query = new GetSpecialtyByDoctorIdQuery(doctorId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpecialtyDto specialty, CancellationToken cancellationToken = default)
        {
            var command = new AddSpecialtyCommand(specialty);

            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            var command = new DeleteSpecialtyCommand(ids);

            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }
        //id
    }
}
