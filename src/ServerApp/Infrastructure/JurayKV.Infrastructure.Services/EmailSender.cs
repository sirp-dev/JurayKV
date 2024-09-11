using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Infrastructure.Services.Configs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using JurayKV.Domain.ValueObjects;
using JurayKV.Domain.Aggregates.SettingAggregate;
using static JurayKV.Domain.Primitives.Enum;
using System.Runtime.Intrinsics.Arm;
using Twilio.TwiML.Messaging;
using PostmarkEmailService;


namespace JurayKV.Infrastructure.Services;

public sealed class EmailSender : IEmailSender
{
    private readonly IConfiguration _configManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IExceptionLogger _exceptionLogger;
    private readonly LoggerLibrary _logger;
    private readonly ISettingRepository _setting;
    private readonly PostmarkClient _postmarkClient;

    public EmailSender(IExceptionLogger exceptionLogger, UserManager<ApplicationUser> userManager, IConfiguration configManager, LoggerLibrary logger, ISettingRepository setting, PostmarkClient postmarkClient)
    {
        _exceptionLogger = exceptionLogger;
        _userManager = userManager;
        _configManager = configManager;
        _logger = logger;
        _setting = setting;
        _postmarkClient = postmarkClient;
        //_postmarkClient = new PostmarkClient(GetServerTokenFromSettings());
    }
    private string GetServerTokenFromSettings()
    {
        return _configManager.GetSection("PostmarkSettings")["ServerToken"];
    }


    public async Task<bool> SendCompanyAsync(string smsMessage, string id, string subject, string compnayname)
    {
        try
        {

            var request = await _userManager.FindByIdAsync(id);

            (string Email, string VerificationCode, string Subject) model = (request.Email, smsMessage, subject);
            // string emailBody = await _viewRenderService.RenderViewToStringAsync("/Pages/EmailTemplate/KvMail", model);
            string emailbody = "{mmm}";
            emailbody = emailbody.Replace("{mmm}", model.VerificationCode);
            try
            {

                string emailTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>//CompanyName//</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">

    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <img src=""//YOUR_LOGO_URL//"" alt=""Company Logo"" width=""200"" style=""display: inline-block;"">
            </td>
        </tr>
        <tr>
            <td style=""background-color: #f4f4f4; padding: 20px;"">
                <h2 style=""color: #333333;"">Dear //Recipient_Name//,</h2>    
<h4 style=""color: #333333;"">//Subject//.</h4>


                <p style=""color: #666666;"">
                    //Body//
                </p>
            </td>
        </tr>
        <tr>
            <td style=""background-color: #333333; color: #ffffff; padding: 10px; text-align: center;"">
                <p>&copy; //Date// //CompanyName// | Contact us at: <a href=""mailto://CompanyEmail//"" style=""color: #ffffff; text-decoration: none;"">//CompanyEmail//</a></p>
            </td>
        </tr>
    </table>

</body>
</html>
";
                emailTemplate = emailTemplate.Replace("//CompanyName//", "Koboview");
                emailTemplate = emailTemplate.Replace("//YOUR_LOGO_URL//", "https://koboview.com/img/Koboview-logo-main.png");
                emailTemplate = emailTemplate.Replace("//Subject//", model.Subject.Replace("\r\n", ""));
                emailTemplate = emailTemplate.Replace("//Body//", emailbody);
                emailTemplate = emailTemplate.Replace("//CompanyEmail//", "help@koboview.com");
                emailTemplate = emailTemplate.Replace("//Date//", DateTime.UtcNow.Year.ToString());
                emailTemplate = emailTemplate.Replace("//Recipient_Name//", compnayname);


                PostmarkResponse response = new PostmarkResponse();
                var message = new PostmarkMessage
                {
                    From = "help@koboview.com",
                    To = model.Email,
                    Subject = subject,
                    HtmlBody = emailTemplate
                };


                response = await _postmarkClient.SendMessageAsync(message);
                if (response.Status == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        catch (Exception exception)
        {
            await _exceptionLogger.LogAsync(exception, "");
            return false;
        }
    }


    public async Task<bool> SendAsync(string smsMessage, string id, string subject)
    {
        try
        {

            var request = await _userManager.FindByIdAsync(id);

            (string Email, string VerificationCode, string Subject) model = (request.Email, smsMessage, subject);
            // string emailBody = await _viewRenderService.RenderViewToStringAsync("/Pages/EmailTemplate/KvMail", model);
            string emailbody = "{mmm}";
            emailbody = emailbody.Replace("{mmm}", model.VerificationCode);
            try
            {

                string emailTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>//CompanyName//</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">

    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <img src=""//YOUR_LOGO_URL//"" alt=""Company Logo"" width=""200"" style=""display: inline-block;"">
            </td>
        </tr>
        <tr>
            <td style=""background-color: #f4f4f4; padding: 20px;"">
                <h2 style=""color: #333333;"">HELLO //Recipient_Name//,</h2>    
<h4 style=""color: #333333;"">//Subject//.</h4>


                <p style=""color: #666666;"">
                    //Body//
                </p>
            </td>
        </tr>
        <tr>
            <td style=""background-color: #333333; color: #ffffff; padding: 10px; text-align: center;"">
                <p>&copy; //Date// //CompanyName// | Contact us at: <a href=""mailto://CompanyEmail//"" style=""color: #ffffff; text-decoration: none;"">//CompanyEmail//</a></p>
            </td>
        </tr>
    </table>

</body>
</html>
";
                emailTemplate = emailTemplate.Replace("//CompanyName//", "Koboview");
                emailTemplate = emailTemplate.Replace("//YOUR_LOGO_URL//", "https://koboview.com/img/Koboview-logo-main.png");
                emailTemplate = emailTemplate.Replace("//Subject//", model.Subject.Replace("\r\n", ""));
                emailTemplate = emailTemplate.Replace("//Body//", emailbody);
                emailTemplate = emailTemplate.Replace("//CompanyEmail//", "help@koboview.com");
                emailTemplate = emailTemplate.Replace("//Date//", DateTime.UtcNow.Year.ToString());
                emailTemplate = emailTemplate.Replace("//Recipient_Name//", request.SurName);


                PostmarkResponse response = new PostmarkResponse();
                var message = new PostmarkMessage
                {
                    From = "help@koboview.com",
                    To = model.Email,
                    Subject = subject,
                    HtmlBody = emailTemplate
                };

               
                    response = await _postmarkClient.SendMessageAsync(message);
                if (response.Status == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                    //using (var message = new MailMessage())
                    //{
                    //    message.To.Add(new MailAddress(model.Email));
                    //    //message.To.Add(new MailAddress("onwukaemeka41@gmail.com"));

                    //    message.Subject = model.Subject.Replace("\r\n", "");
                    //    message.Body = emailTemplate;
                    //    message.IsBodyHtml = true;


                    //    message.From = new MailAddress("noreply@koboview.com", "koboview"); //IMPORTANT: This must be same as your smtp authentication address.

                    //        SmtpClient smtp = new SmtpClient("mail.koboview.com");
                    //        NetworkCredential Credentials = new NetworkCredential("noreply@koboview.com", "Admin@123");
                    //        smtp.UseDefaultCredentials = false;
                    //        smtp.Credentials = Credentials;
                    //        smtp.Port = 25;    //alternative port number is 8889
                    //        smtp.EnableSsl = false;
                    //        smtp.Send(message);
                    //         return true;

                    //}
                    

            }
            catch (Exception ex)
            {

                return false;
            }
        }
        catch (Exception exception)
        {
            await _exceptionLogger.LogAsync(exception, "");
            return false;
        }
    }

    
    public async Task<bool> SendEmailAsync(string smsMessage, string email, string subject)
    {
        try
        {

            var request = await _userManager.FindByEmailAsync(email);

            (string Email, string VerificationCode, string Subject) model = (request.Email, smsMessage, subject);
            // string emailBody = await _viewRenderService.RenderViewToStringAsync("/Pages/EmailTemplate/KvMail", model);
            string emailbody = "{mmm}";
            emailbody = emailbody.Replace("{mmm}", model.VerificationCode);
            try
            {

                string emailTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>//CompanyName//</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">

    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <img src=""//YOUR_LOGO_URL//"" alt=""Company Logo"" width=""200"" style=""display: inline-block;"">
            </td>
        </tr>
        <tr>
            <td style=""background-color: #f4f4f4; padding: 20px;"">
                <h2 style=""color: #333333;"">HELLO,</h2>    
<h4 style=""color: #333333;"">//Subject//.</h4>


                <p style=""color: #666666;"">
                    //Body//
                </p>
            </td>
        </tr>
        <tr>
            <td style=""background-color: #333333; color: #ffffff; padding: 10px; text-align: center;"">
                <p>&copy; //Date// //CompanyName// | Contact us at: <a href=""mailto://CompanyEmail//"" style=""color: #ffffff; text-decoration: none;"">//CompanyEmail//</a></p>
            </td>
        </tr>
    </table>

</body>
</html>
";
                emailTemplate = emailTemplate.Replace("//CompanyName//", "Koboview");
                emailTemplate = emailTemplate.Replace("//YOUR_LOGO_URL//", "https://koboview.com/img/Koboview-logo-main.png");
                emailTemplate = emailTemplate.Replace("//Subject//", model.Subject.Replace("\r\n", ""));
                emailTemplate = emailTemplate.Replace("//Body//", emailbody);
                emailTemplate = emailTemplate.Replace("//CompanyEmail//", "help@koboview.com");
                emailTemplate = emailTemplate.Replace("//Date//", DateTime.UtcNow.Year.ToString());
                emailTemplate = emailTemplate.Replace("//Recipient_Name//", "");


                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress(model.Email));

                    message.Subject = model.Subject.Replace("\r\n", "");
                    message.Body = emailTemplate;
                    message.IsBodyHtml = true;
                    //message.From = new MailAddress("noreply@koboview.com", "Koboview");
                    //using (var client = new SmtpClient("smtp.office365.com"))
                    //{
                    //    client.UseDefaultCredentials = false;
                    //    client.Port = 587;
                    //    client.Credentials = new NetworkCredential("noreply@koboview.com", "ahambuPeter@247");
                    //    client.EnableSsl = true;

                    //    try
                    //    {
                    //        await client.SendMailAsync(message); // Email sent
                    //        _logger.Log($"outlook mail sent to {model.Email}");

                    //        return true;
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        _logger.Log($"outlook failed mail to {model.Email} {e.ToString()}");
                    //        // Email not sent, log exception
                    //        return false;
                    //    }

                    //}

                    message.From = new MailAddress("help@koboview.com", "Koboview");
                    using (var client = new SmtpClient("smtppro.zoho.com"))
                    {
                        client.UseDefaultCredentials = false;
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("help@koboview.com", "Nation@0904");
                        client.EnableSsl = true;

                        try
                        {
                            await client.SendMailAsync(message); // Email sent
                            _logger.Log($" mail sent to {model.Email}");

                            return true;
                        }
                        catch (Exception e)
                        {
                            _logger.Log($"outlook failed mail to {model.Email} {e.ToString()}");
                            // Email not sent, log exception
                            return false;
                        }

                    }
                }


            }
            catch (Exception ex)
            {

                return false;
            }
        }
        catch (Exception exception)
        {
            await _exceptionLogger.LogAsync(exception, "");
            return false;
        }
    }
}
