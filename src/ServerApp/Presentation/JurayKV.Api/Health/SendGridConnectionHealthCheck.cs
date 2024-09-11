using System;
using System.Threading;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JurayKV.Api;

public class SendGridConnectionHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
