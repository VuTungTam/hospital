using AutoMapper;
using Hospital.Application.Commands.HealthFacilities;
using Hospital.Application.Commands.Specialties;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Queries.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthFacilities
{
    public class HealthFacilityController : ApiBaseController
    {
        private IMapper _mapper;
        private readonly IFacilityCategoryReadRepository _facilityCategoryReadRepository;
        public HealthFacilityController(
            IMediator mediator,
            IMapper mapper,
            IFacilityCategoryReadRepository facilityCategoryReadRepository
            ) : base(mediator)
        {
            _mapper = mapper;
            _facilityCategoryReadRepository = facilityCategoryReadRepository;
        }

        [HttpGet("type"), AllowAnonymous]
        public async Task<IActionResult> GetAllFacilityType(CancellationToken cancellationToken = default)
        {
            var types = await _facilityCategoryReadRepository.GetAsync(cancellationToken: cancellationToken);

            var dtos = _mapper.Map<List<FacilityTypeDto>>(types);

            return Ok(new SimpleDataResult { Data = dtos });
        }

        [HttpGet("pagination"), AllowAnonymous]
        public async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long typeId = 0,
            HealthFacilityStatus status = HealthFacilityStatus.None,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetHealthFacilityPagingQuery(pagination, typeId, status);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetFacilityByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
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
