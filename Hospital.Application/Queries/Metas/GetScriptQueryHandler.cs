using AutoMapper;
using Hospital.Application.Dtos.Metas;
using Hospital.Application.Repositories.Interfaces.Metas;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Metas
{
    public class GetScriptQueryHandler : BaseQueryHandler, IRequestHandler<GetScriptQuery, ScriptDto>
    {
        private readonly IScriptReadRepository _scriptReadRepository;

        public GetScriptQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IScriptReadRepository scriptReadRepository
        ) : base(authService, mapper, localizer)
        {
            _scriptReadRepository = scriptReadRepository;
        }

        public async Task<ScriptDto> Handle(GetScriptQuery request, CancellationToken cancellationToken)
        {
            var script = await _scriptReadRepository.ReadAsync(cancellationToken);

            return _mapper.Map<ScriptDto>(script);
        }
    }
}
