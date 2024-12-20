﻿using Hospital.SharedKernel.Application.Enums;
using Microsoft.AspNetCore.Http;

namespace Hospital.SharedKernel.Runtime.ExecutionContext
{
    public interface IExecutionContext
    {
        string TraceId { get; }

        string AccessToken { get; }

        string Uid { get; }

        string Username { get; }

        string Permission { get; }

        long BranchId { get; }

        List<long> BranchIds { get; }

        long UserId { get; }

        //int Shard { get; }

        bool IsAnonymous { get; }

        AccountType AccountType { get; }

        HttpContext HttpContext { get; }

        void UpdateContext(string accessToken);

        void MakeAnonymousRequest();

        bool IsSuperAdmin();
    }
}
