using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Infrastructures
{ 
    [SingletonService]
    public interface IWhatsappOtp
    {
        Task<bool> SendAsync(string smsMessage, string id);
    }
}
