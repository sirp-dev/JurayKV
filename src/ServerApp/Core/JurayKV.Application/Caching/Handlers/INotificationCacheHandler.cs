﻿using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface INotificationCacheHandler
    {
        Task RemoveDetailsByIdAsync(Guid modelId);
        Task RemoveGetAsync(Guid modelId);
        Task RemoveListAsync();
        
    }
 
}
