﻿using System;
using System.Threading.Tasks;
using JurayKV.Application.Caching.Handlers;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;

namespace JurayKV.Persistence.Cache.Handlers;

internal sealed class EmployeeCacheHandler : IEmployeeCacheHandler
{
    private readonly IDistributedCache _distributedCache;

    public EmployeeCacheHandler(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task RemoveDetailsByIdAsync(Guid employeeId)
    {
        string detailsKey = EmployeeCacheKeys.GetDetailsKey(employeeId);
        await _distributedCache.RemoveAsync(detailsKey);
    }
}