using Hospital.Application.Commands.Doctors;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Models;
using Hospital.Application.Models.Doctors;
using Hospital.Application.Queries.Doctors;
using Hospital.Application.Queries.Sequences;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.Controllers.Doctors
{
    public class DoctorController : ApiBaseController
    {
        public DoctorController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet("filterable")]
        public IActionResult GetFilterable() => GetFilterable<Doctor>();

        [HttpGet("enums"), AllowAnonymous]
        public IActionResult GetEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<AccountStatus>(noneOption) });

        [HttpGet("enums/title"), AllowAnonymous]
        public IActionResult GetTitleEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<DoctorTitle>(noneOption) });

        [HttpGet("enums/degree"), AllowAnonymous]
        public IActionResult GetDegreeEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<DoctorDegree>(noneOption) });

        [HttpGet("enums/rank"), AllowAnonymous]
        public IActionResult GetRankEnums(string noneOption) => Ok(new SimpleDataResult { Data = EnumerationExtensions.ToValues<DoctorRank>(noneOption) });

        [HttpGet("sequence")]
        public async Task<IActionResult> GetSequence(CancellationToken cancellationToken = default)
        {
            var table = new Doctor().GetTableName();
            return Ok(new SimpleDataResult { Data = await _mediator.Send(new GetSequenceQuery(table), cancellationToken) });
        }

        [HttpPost("public/pagination"), AllowAnonymous]
        public async Task<IActionResult> GettPublicDoctorPagination(int page, int size, int state, [FromBody] FilterDoctorRequest request, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetPublicDoctorsPaginationQuery(pagination, request, (AccountStatus)state);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpPost("pagination")]
        public async Task<IActionResult> GetDoctorPagination(int page, int size, int state, [FromBody] FilterDoctorRequest request, string search = "", string asc = "", string desc = "", CancellationToken cancellationToken = default)
        {
            var pagination = new Pagination(page, size, search, QueryType.Contains, asc, desc);
            var query = new GetDoctorsPaginationQuery(pagination, (AccountStatus)state, request);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(new ServiceResult { Data = result.Data, Total = result.Total });
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetDoctorByIdQuery(id);
            var doctor = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = doctor });
        }

        [HttpGet("public/{id}"), AllowAnonymous]
        public virtual async Task<IActionResult> GetPublicById(long id, CancellationToken cancellationToken = default)
        {
            var query = new GetPublicDoctorByIdQuery(id);
            var doctor = await _mediator.Send(query, cancellationToken);

            return Ok(new SimpleDataResult { Data = doctor });
        }

        [HttpPost]
        public async Task<IActionResult> Add(DoctorDto doctor, CancellationToken cancellationToken = default)
        {
            var command = new AddDoctorCommand(doctor);
            return Ok(new SimpleDataResult { Data = await _mediator.Send(command, cancellationToken) });
        }

        [HttpPut("change-profile-for-doctor")]
        public async Task<IActionResult> Update(DoctorDto doctor, CancellationToken cancellationToken = default)
        {
            var command = new UpdateDoctorCommand(doctor);
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpPut("change-password-for-doctor")]
        public async Task<IActionResult> ChangePasswordForDoctor(UserPwdModel model, CancellationToken cancellationToken = default)
        {
            var command = new UpdateDoctorPasswordCommand(model);
            await _mediator.Send(command, cancellationToken);
            return Ok(new BaseResponse());
        }

        [HttpPut("update-status/{id}/{status}")]
        public async Task<IActionResult> UpdateState(long id, AccountStatus status, CancellationToken cancellationToken = default)
        {
            var command = new UpdateDoctorStatusCommand(id, status);
            await _mediator.Send(command, cancellationToken);

            return Ok(new BaseResponse());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(List<long> ids, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteDoctorsCommand(ids), cancellationToken);
            return Ok(new BaseResponse());
        }

    }
}
