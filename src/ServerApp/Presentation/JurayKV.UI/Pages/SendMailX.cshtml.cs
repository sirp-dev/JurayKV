using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayKV.UI.Pages
{
        public class SendMailXModel : PageModel
    {
        private readonly IEmailSender _sender;
        private readonly UserManager<ApplicationUser> _userManager;


        public SendMailXModel(UserManager<ApplicationUser> userManager, IEmailSender sender)
        {
            // Set the path to the "datalog.txt" file
            _userManager = userManager;
            _sender = sender;
        }

        public string LogContent { get; private set; }
        [BindProperty]
        public bool Passkk { get; set; } = false;
        public IActionResult OnGet()
        {
             
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int take, int skip)
        {
            int sent = 0;
           var users = await _userManager.Users
                .OrderBy(u => u.CreationUTC)
                .Skip(skip).Take(take).ToListAsync();
            string mss = $"We recently experienced a temporary disruption in our email verification system, resulting in some users not receiving confirmation emails. The issue has been resolved, and we apologize for any inconvenience. To ensure continued access to your account, kindly log in to www.koboview.com and reverify your email address. We appreciate your prompt attention to this matter.";
            foreach(var user in users)
            {
               //bool Result = await _sender.SendAsync(mss, user.Id.ToString(), "KOBOVIEW VERIFICATION");
               bool Result = await _sender.SendEmailAsync(mss, user.Email, "KOBOVIEW VERIFICATION");
                if (Result)
                {
                    sent++;
                }
            }
            LogContent = sent.ToString();
            return Page();
        }

         

    }

}
