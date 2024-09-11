using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Services
{
    public static class ModelStateExtensions
    {
        public static string GetAllErrors(this ModelStateDictionary modelState)
        {
            return string.Join(" ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
    }
}
