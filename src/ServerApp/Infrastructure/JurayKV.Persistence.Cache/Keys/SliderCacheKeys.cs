using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
    internal static class SliderCacheKeys
    {
        public static string ListKey => "SliderList";
        
        public static string GetKey(Guid sliderId)
        {
            return $"Slider-{sliderId}";
        }
         
    }
     
}
