using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Metas;
using Hospital.Domain.Entities.Metas;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Metas
{
    public class UpdateScriptCommandHandler : BaseCommandHandler, IRequestHandler<UpdateScriptCommand>
    {
        private readonly IScriptReadRepository _scriptReadRepository;
        private readonly IScriptWriteRepository _scriptWriteRepository;

        public UpdateScriptCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IScriptReadRepository scriptReadRepository,
            IScriptWriteRepository scriptWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _scriptReadRepository = scriptReadRepository;
            _scriptWriteRepository = scriptWriteRepository;
        }

        public async Task<Unit> Handle(UpdateScriptCommand request, CancellationToken cancellationToken)
        {
            var script = await _scriptReadRepository.ReadAsync(cancellationToken);
            if (script == null)
            {
                SetScript(ref script, request);
                await _scriptWriteRepository.AddAsync(script, cancellationToken);
            }
            else
            {
                request.Script.Id = script.Id.ToString();
                SetScript(ref script, request);

                await _scriptWriteRepository.UpdateAsync(script, cancellationToken: cancellationToken);
            }

            return Unit.Value;
        }

        private void SetScript(ref Script script, UpdateScriptCommand request)
        {
            script = _mapper.Map<Script>(request.Script);
            //script.AddDomainEvent(new UpdateScriptDomainEvent(script));

            if (!string.IsNullOrEmpty(script.Header))
            {
                script.Header = script.Header.Trim();
            }

            if (!string.IsNullOrEmpty(script.Body))
            {
                script.Body = script.Body.Trim();
            }

            if (!string.IsNullOrEmpty(script.Footer))
            {
                script.Footer = script.Footer.Trim();
            }
        }
    }
}
