using JurayKV.Application.Commands.IdentityCommands.UserCommands;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.WebApi.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post(LoginDto loginDTO)
        {
            try
            {
                var command = new LoginCommand { Email = loginDTO.Email, Password = loginDTO.Password };
                var response = await _mediator.Send(command);

                if (response != null)
                {

                    return Ok(response);
                }
                else
                {
                    return Unauthorized("Invalid credentials");
                }
            }
            catch
            {
                return BadRequest("An error occurred in generating the token");
            }

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                CreateUserDto create = new CreateUserDto();
                create.Surname = model.Surname;
                create.Firstname = model.FirstName;
                create.Email = model.Email;
                create.Password = model.Password;
                create.PhoneNumber = model.PhoneNumber;
                create.Role = "User";
                create.RefPhone = model.RefPhone;
                create.State = model.State;
                create.LGA = model.LGA;
                create.Address = model.Address;
                CreateUserManagerCommand command = new CreateUserManagerCommand(create);
                ResponseCreateUserDto result = await _mediator.Send(command);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
                var logincommand = new LoginCommand { Email = model.Email, Password = model.Password };
                var response = await _mediator.Send(command);

                if (response != null)
                {

                    return Ok(response);
                }
            }
            catch (Exception exception)
            {

                return StatusCode(500, "Unable to create account");
            }

            // If we got this far, something failed, return BadRequest with model errors
            return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }
        //[HttpPost("changephonenumber/{userid}")]
        //public async Task<IActionResult> UpdatePhoneNumber(string userId, string oldPhoneNumber, string newPhoneNumber)
        //{
        //    var user = await _userManager.FindByIdAsync(txtd);

        //    if (oldPhoneNumber == user.PhoneNumber)
        //    {
        //        string last10DigitsPhoneNumber = newPhoneNumber.Substring(Math.Max(0, newPhoneNumber.Length - 10));
        //        var checkPhone = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Contains(last10DigitsPhoneNumber));

        //        if (checkPhone == null)
        //        {
        //            user.PhoneNumber = newPhoneNumber;
        //            await _userManager.UpdateAsync(user);
        //            TempData["xsuccess"] = "New Phone Number updated. Request for whatsapp code.";
        //            return Ok(new { xcode = xcode, xmal = xmal, txtd = user.Id });
        //        }
        //        else
        //        {
        //            TempData["xerror"] = "New Phone Number is already in use.";
        //            return BadRequest(new { xcode = xcode, xmal = xmal, txtd = user.Id });
        //        }
        //    }
        //    else
        //    {
        //        TempData["xerror"] = "Incorrect Old Phone Number.";
        //        return BadRequest(new { xcode = xcode, xmal = xmal, txtd = user.Id });
        //    }
        //}
    }
}
