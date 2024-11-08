using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Infrastructure.Services.Cloud.Models;
using Hospital.SharedKernel.Libraries.Attributes;
using Microsoft.AspNetCore.Http;

namespace Hospital.Application.Commands.Images
{
    [RequiredPermission(ActionExponent.Upload)]
    public class UploadImageCommand : BaseCommand<UploadResponse>
    {
        public UploadImageCommand(UploadRequest model)
        {
            Model = model;
        }
        public UploadRequest Model { get; }
    }
}
