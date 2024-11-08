using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Services.Cloud;
using Hospital.SharedKernel.Infrastructure.Services.Cloud.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Images
{
    public class UploadImageCommandHandler : BaseCommandHandler, IRequestHandler<UploadImageCommand, UploadResponse>
    {
        private readonly ICloudinaryService _cloudService;

        public UploadImageCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            ICloudinaryService cloudService
            ) : base(eventDispatcher, authService, localizer)
        {
            _cloudService = cloudService;
        }
        public async Task<UploadResponse> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.File == null || request.Model.Size == 0)
            {
                throw new BadRequestException(_localizer["images_must_not_be_empty"]);
            }
            var imageUrl = await _cloudService.UploadAsync(request.Model, cancellationToken);
            return imageUrl;
        }
    }
}
