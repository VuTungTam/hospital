﻿using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Libraries.Attributes;
using MediatR;

namespace Hospital.SharedKernel.Application.CQRS.Commands
{
    [RequiredPermission(ActionExponent.Delete)]
    public class DeleteCommand<T, TResponse> : BaseCommand<TResponse> where T : BaseEntity
    {
        public List<long> Ids { get; }

        public DeleteCommand(List<long> ids)
        {
            Ids = ids;
        }

        public bool IsValidIds() => Ids.Any() && Ids.All(id => id > 0);
    }
    public class DeleteCommand<T> : DeleteCommand<T, Unit> where T : BaseEntity
    {
        public DeleteCommand(List<long> ids) : base(ids)
        {
        }
    }
}
