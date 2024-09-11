using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Pages
{
    public class HelpModel : PageModel
    {
        public string RefX { get; set; }
        public void OnGet(string refx = null)
        {
            RefX = refx;
        }
    }
}
