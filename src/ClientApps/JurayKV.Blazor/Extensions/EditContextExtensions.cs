using JurayKV.Blazor.Common;
using Microsoft.AspNetCore.Components.Forms;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Blazor.Extensions;

public static class EditContextExtensions
{
    public static void AddBootstrapValidationClassProvider(this EditContext editContext)
    {
        editContext.ThrowIfNull(nameof(editContext));

        editContext.SetFieldCssClassProvider(new BootstrapValidationClassProvider());
    }
}
