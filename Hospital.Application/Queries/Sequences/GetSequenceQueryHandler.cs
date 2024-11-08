using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Sequences
{
    public class GetSequenceQueryHandler : BaseQueryHandler, IRequestHandler<GetSequenceQuery, string>
    {
        private readonly ISequenceRepository _sequenceRepository;

        public GetSequenceQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ISequenceRepository sequenceRepository
        ) : base(authService, mapper, localizer)
        {
            _sequenceRepository = sequenceRepository;
        }

        public async Task<string> Handle(GetSequenceQuery request, CancellationToken cancellationToken)
        {
            var sequence = await _sequenceRepository.GetSequenceAsync(request.Table, cancellationToken);
            return sequence.ValueString;
        }
    }
}

