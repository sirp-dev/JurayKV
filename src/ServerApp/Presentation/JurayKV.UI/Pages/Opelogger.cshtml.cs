using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Pages
{
    public class OpeloggerModel : PageModel
    {
        private readonly string logFilePath;

        public OpeloggerModel()
        {
            // Set the path to the "datalog.txt" file
            logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "datalog.txt");
        }

        public string LogContent { get; private set; }
        [BindProperty]
        public bool Passkk { get; set; } = false;
        public IActionResult OnGet()
        {
            if (TempData["valide"] != null && (bool)TempData["valide"])
            {
                Passkk = true;
                // Read the content of the log file
                if (System.IO.File.Exists(logFilePath))
                {
                    LogContent = System.IO.File.ReadAllText(logFilePath);
                }
                else
                {
                    LogContent = "Log file not found.";
                }
            }
            else
            {
                // Code not provided, redirect to another page or display an error
                Passkk = false;
            }



            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string pass)
        {
            if (pass == "08165680904")
            {
                TempData["valide"] = true;
            }

            return RedirectToPage();
        }


        public IActionResult OnPostClearLog()
        {
            // Clear the content of the log file
            System.IO.File.WriteAllText(logFilePath, string.Empty);

            // Redirect back to the same page after clearing the log
            return RedirectToPage();
        }

    }

}
