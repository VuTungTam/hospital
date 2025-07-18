﻿using AutoMapper;
using Hospital.Application.Dtos.Feedbacks;
using Hospital.Application.Repositories.Interfaces.Feedbacks;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.Feedbacks;
using Hospital.Domain.Specifications.Auths;
using Hospital.Domain.Specifications.Feedbacks;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Feedbacks
{
    public class GetFeedbacksPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetFeedbacksPaginationQuery, PaginationResult<FeedbackDto>>
    {
        private readonly IFeedbackReadRepository _feedbackReadRepository;

        public GetFeedbacksPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IFeedbackReadRepository feedbackReadRepository
            ) : base(authService, mapper, localizer)
        {
            _feedbackReadRepository = feedbackReadRepository;
        }
        public async Task<PaginationResult<FeedbackDto>> Handle(GetFeedbacksPaginationQuery request, CancellationToken cancellationToken)
        {
            ISpecification<Feedback> spec = new ExpressionSpecification<Feedback>(x => true);

            if (request.ServiceId > 0)
            {
                spec = spec.And(new GetFeedbackByServiceIdSpecification(request.ServiceId));
            }
            if (request.FacilityId > 0)
            {
                spec = spec.And(new GetFeedbackByFacilityIdSpecification(request.FacilityId));
            }
            if (request.Star > 0)
            {
                spec = spec.And(new GetFeedbackByStarSpecification(request.Star));
            }
            var option = new QueryOption
            {
                IgnoreOwner = true,
                Includes = new string[] { nameof(Feedback.Booking) }
            };

            var result = await _feedbackReadRepository.GetPaginationAsync(request.Pagination, spec, option, cancellationToken: cancellationToken);

            var dtos = new List<FeedbackDto>();
            foreach (var feedback in result.Data)
            {
                var dto = _mapper.Map<FeedbackDto>(feedback);
                dto.FacilityNameVn = feedback.Booking.FacilityNameVn;
                dto.FacilityNameEn = feedback.Booking.FacilityNameEn;
                dtos.Add(dto);
            }


            return new PaginationResult<FeedbackDto>(dtos, result.Total);
        }
    }
}
