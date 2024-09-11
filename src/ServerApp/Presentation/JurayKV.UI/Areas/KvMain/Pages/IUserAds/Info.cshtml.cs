using JurayKV.Application;
using JurayKV.Application.Commands.IdentityKvAdCommands;
using JurayKV.Application.Commands.KvPointCommands;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUserAds
{
    [Authorize(Policy = Constants.PointPolicy)]
    public class InfoModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [BindProperty]
        public IdentityKvAdDetailsDto IdentityKvAdDetailsDto { get; set; }

        [BindProperty]
        public int PointOne { get; set; }

        [BindProperty]
        public int PointTwo { get; set; }

        [BindProperty]
        public int PointThree { get; set; }

        public CompanyDetailsDto CompanyDetailsDto { get; set; }
        public WalletDetailsDto WalletDetailsDto { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetIdentityKvAdByIdQuery command = new GetIdentityKvAdByIdQuery(id);
                IdentityKvAdDetailsDto = await _mediator.Send(command);


                GetCompanyByIdQuery companycomand = new GetCompanyByIdQuery(IdentityKvAdDetailsDto.CompanyId);
                CompanyDetailsDto = await _mediator.Send(companycomand);

                GetWalletUserByIdQuery walletcommand = new GetWalletUserByIdQuery(CompanyDetailsDto.UserId);
                WalletDetailsDto = await _mediator.Send(walletcommand);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch users";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            GetIdentityKvAdByIdQuery command = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
            var updateAds = await _mediator.Send(command);


            //check if amount in the company wllet can fund the advert
            GetWalletUserByIdQuery walletcommand = new GetWalletUserByIdQuery(CompanyUserId);
            WalletDetailsDto = await _mediator.Send(walletcommand);



            string userId = _userManager.GetUserId(HttpContext.User);
            var userinfo = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(userinfo);

            if (roles.Contains(Constants.AdminOne) || roles.Contains(Constants.SuperAdminPolicy))
            {
                if (WalletDetailsDto.Amount < PointOne)
                {
                    try
                    {
                        GetIdentityKvAdByIdQuery xcommand = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
                        IdentityKvAdDetailsDto = await _mediator.Send(xcommand);

                        GetCompanyByIdQuery companycomand = new GetCompanyByIdQuery(IdentityKvAdDetailsDto.CompanyId);
                        CompanyDetailsDto = await _mediator.Send(companycomand);
 
                        TempData["error"] = "Point Exceed Balance";
                        return Page();
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "unable to fetch users";
                        return RedirectToPage("/Index");
                    }
                }
                updateAds.ResultOne = PointOne;
                updateAds.Activity = updateAds.Activity + "<br> admin: " + userinfo.SurName + " " + userinfo.FirstName + "update result one with " + PointOne;
            }

            if (roles.Contains(Constants.AdminTwo) || roles.Contains(Constants.SuperAdminPolicy))
            {
                if (WalletDetailsDto.Amount < PointTwo)
                {
                    try
                    {
                        GetIdentityKvAdByIdQuery xcommand = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
                        IdentityKvAdDetailsDto = await _mediator.Send(xcommand);

                        GetCompanyByIdQuery companycomand = new GetCompanyByIdQuery(IdentityKvAdDetailsDto.CompanyId);
                        CompanyDetailsDto = await _mediator.Send(companycomand);

                         
                        TempData["error"] = "Point Exceed Balance";
                        return Page();
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "unable to fetch users";
                        return RedirectToPage("/Index");
                    }
                }
                updateAds.ResultTwo = PointTwo;
                updateAds.Activity = updateAds.Activity + "<br> admin: " + userinfo.SurName + " " + userinfo.FirstName + "update result one with " + PointTwo;
            }

            if (roles.Contains(Constants.AdminThree) || roles.Contains(Constants.SuperAdminPolicy))
            {
                if (WalletDetailsDto.Amount < PointThree)
                {
                    try
                    {
                        GetIdentityKvAdByIdQuery xcommand = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
                        IdentityKvAdDetailsDto = await _mediator.Send(xcommand);

                        GetCompanyByIdQuery companycomand = new GetCompanyByIdQuery(IdentityKvAdDetailsDto.CompanyId);
                        CompanyDetailsDto = await _mediator.Send(companycomand);

                         
                        TempData["error"] = "Point Exceed Balance";
                        return Page();
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "unable to fetch users";
                        return RedirectToPage("/Index");
                    }
                }
                updateAds.ResultThree = PointThree;
                updateAds.Activity = updateAds.Activity + "<br> admin: " + userinfo.SurName + " " + userinfo.FirstName + "update result one with " + PointThree;
            }

            UpdateIdentityKvAdPointCommand updatecommand = new UpdateIdentityKvAdPointCommand(updateAds.Id, updateAds.ResultOne, updateAds.ResultTwo, updateAds.ResultThree, updateAds.Activity);
            await _mediator.Send(updatecommand);
            return RedirectToPage("./Info", new { id = IdentityKvAdDetailsDto.Id });
        }

        [BindProperty]
        public string PointChoose { get; set; }

        [BindProperty]
        public Guid CompanyUserId { get; set; }
        public async Task<IActionResult> OnPostUpdatePoint()
        {

            GetIdentityKvAdByIdQuery command = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
            var updateAds = await _mediator.Send(command);

            //check if amount in the company wllet can fund the advert
            GetWalletUserByIdQuery walletcommand = new GetWalletUserByIdQuery(CompanyUserId);
            WalletDetailsDto = await _mediator.Send(walletcommand);

            KvPoint kp = new KvPoint();
            kp.UserId = updateAds.UserId;
            if (PointChoose == "First")
            {
                kp.Point = updateAds.ResultOne;
                if (WalletDetailsDto.Amount < updateAds.ResultOne)
                {
                    try
                    {
                        GetIdentityKvAdByIdQuery xcommand = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
                        IdentityKvAdDetailsDto = await _mediator.Send(xcommand);

                        GetCompanyByIdQuery companycomand = new GetCompanyByIdQuery(IdentityKvAdDetailsDto.CompanyId);
                        CompanyDetailsDto = await _mediator.Send(companycomand);

                        TempData["error"] = "Point Exceed Balance";
                        return Page();
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "unable to fetch users";
                        return RedirectToPage("/Index");
                    }
                }
            }
            else if (PointChoose == "Second")
            {
                kp.Point = updateAds.ResultTwo;
                if (WalletDetailsDto.Amount < updateAds.ResultTwo)
                {
                    try
                    {
                        GetIdentityKvAdByIdQuery xcommand = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
                        IdentityKvAdDetailsDto = await _mediator.Send(xcommand);

                        GetCompanyByIdQuery companycomand = new GetCompanyByIdQuery(IdentityKvAdDetailsDto.CompanyId);
                        CompanyDetailsDto = await _mediator.Send(companycomand);

                        TempData["error"] = "Point Exceed Balance";
                        return Page();
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "unable to fetch users";
                        return RedirectToPage("/Index");
                    }
                }
            }
            else if (PointChoose == "Third")
            {
                kp.Point = updateAds.ResultThree;
                if (WalletDetailsDto.Amount < updateAds.ResultThree)
                {
                    try
                    {
                        GetIdentityKvAdByIdQuery xcommand = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
                        IdentityKvAdDetailsDto = await _mediator.Send(xcommand);

                        GetCompanyByIdQuery companycomand = new GetCompanyByIdQuery(IdentityKvAdDetailsDto.CompanyId);
                        CompanyDetailsDto = await _mediator.Send(companycomand);

                        TempData["error"] = "Point Exceed Balance";
                        return Page();
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "unable to fetch users";
                        return RedirectToPage("/Index");
                    }
                }
            }
            else
            {
                try
                {
                    GetIdentityKvAdByIdQuery xcommand = new GetIdentityKvAdByIdQuery(IdentityKvAdDetailsDto.Id);
                    IdentityKvAdDetailsDto = await _mediator.Send(xcommand);

                    GetCompanyByIdQuery companycomand = new GetCompanyByIdQuery(IdentityKvAdDetailsDto.CompanyId);
                    CompanyDetailsDto = await _mediator.Send(companycomand);

                    TempData["error"] = "kindly choose an option";
                    return Page();
                }
                catch (Exception ex)
                {
                    TempData["error"] = "unable to fetch users";
                    return RedirectToPage("/Index");
                }

            }
            kp.Status = Domain.Primitives.Enum.EntityStatus.Successful;
            kp.PointHash = Guid.NewGuid().ToString();
            kp.IdentityKvAdId = updateAds.Id;
            CreateKvPointCommand updatecommand = new CreateKvPointCommand(kp.UserId, kp.IdentityKvAdId, kp.Status, kp.Point, kp.PointHash);
            await _mediator.Send(updatecommand);
            return RedirectToPage("./Info", new { id = IdentityKvAdDetailsDto.Id });
        }
    }

}
