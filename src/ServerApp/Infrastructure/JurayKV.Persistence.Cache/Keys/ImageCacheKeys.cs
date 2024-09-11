using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.Cache.Keys
{
     internal static class ImageCacheKeys
    {
        public static string ListKey => "ImageList";
        public static string DropdownListKey => "DropdownImageList";
 
        public static string GetKey(Guid imageId)
        {
            return $"Image-{imageId}";
        }
         
    }
}
