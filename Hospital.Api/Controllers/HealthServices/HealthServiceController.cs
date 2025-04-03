using AutoMapper;
using Hospital.Application.Commands.HealthServices;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Queries.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.HealthServices
{
    public class HealthServiceController : ApiBaseController
    {
        private readonly IServiceTypeReadRepository _serviceTypeReadRepository;
        private readonly IMapper _mapper;
        public HealthServiceController(
            IMediator mediator,
            IServiceTypeReadRepository serviceTypeReadRepository,
            IMapper mapper
            ) : base(mediator)
        {
            _serviceTypeReadRepository = serviceTypeReadRepository;
            _mapper = mapper;
        }

        [HttpGet("type"), AllowAnonymous]
        public async Task<IActionResult> GetAllServiceType(CancellationToken cancellationToken = default)
        {
            var types = await _serviceTypeReadRepository.GetAsync(cancellationToken: cancellationToken);

            var dtos = _mapper.Map<List<ServiceTypeDto>>(types);

            return Ok(new SimpleDataResult { Data = dtos });
        }
        [HttpGet("pagination")]
        public virtual async Task<IActionResult> GetPagination(
            int page,
            int size,
            string search = "",
            string asc = "",
            string desc = "",
            long typeId = 0,
            long facilityId = 0,
            long specialtyId = 0,
            HealthServiceStatus status = HealthServiceStatus.None,
            CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);

            var query = new GetHealthServicePagingQuery(pagination, facilityId, typeId, specialtyId, status);

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetHealthServiceByIdQuery(id);

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
