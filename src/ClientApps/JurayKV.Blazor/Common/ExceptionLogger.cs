﻿using System;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Blazor.Common;

[ScopedService]
public class ExceptionLogger
{
    public async Task LogAsync(Exception exception)
    {
        if (exception != null)
        {
            Console.WriteLine(exception);

        }

        await Task.CompletedTask;
    }
}
