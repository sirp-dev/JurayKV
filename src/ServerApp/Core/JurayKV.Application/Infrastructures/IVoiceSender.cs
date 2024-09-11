using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Infrastructures
{
     
    [SingletonService]
    public interface IVoiceSender
    {
        Task<bool> SendAsync(string voiceMessage, string id);
    }
}
