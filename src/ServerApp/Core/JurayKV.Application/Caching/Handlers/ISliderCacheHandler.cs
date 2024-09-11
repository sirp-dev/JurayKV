﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Caching.Handlers
{
    public interface ISliderCacheHandler
    {
        Task RemoveGetByIdAsync(Guid sliderId);
        Task RemoveListAsync();

    }

}