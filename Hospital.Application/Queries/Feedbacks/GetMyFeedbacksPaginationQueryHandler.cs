using AutoMapper;
using Hospital.Application.Dtos.Feedbacks;
using Hospital.Application.Repositories.Interfaces.Feedbacks;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Domain.Specifications.Feedbacks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Specifications;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Feedbacks
{
    public class GetMyFeedbacksPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetMyFeedbacksPaginationQuery, PaginationResult<FeedbackDto>>
    {
        private readonly IFeedbackReadRepository _feedbackReadRepository;
        public GetMyFeedbacksPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IFeedbackReadRepository feedbackReadRepository
            ) : base(authService, mapper, localizer)
        {
            _feedbackReadRepository = feedbackReadRepository;
        }


        public async Task<PaginationResult<FeedbackDto>> Handle(GetMyFeedbacksPaginationQuery request, CancellationToken cancellationToken)
        {

            var result = await _feedbackReadRepository.GetPaginationAsync(request.Pagination, null, cancellationToken: cancellationToken);

            var dto = _mapper.Map<List<FeedbackDto>>(result.Data);

            return new PaginationResult<FeedbackDto>(dto, result.Total);
        }
    }
}
