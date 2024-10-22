using HolaHousing_BE.Services.EmailServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        [HttpGet("SendRegularEmail")]
        public void SendRegularEmail(string toEmail, string subject, string message, string username)
        {
            SendEmail se = new SendEmail();
            se.SendEmailRegularAsync(toEmail,subject,message,username);
        }
        [HttpGet("SendOTP")]
        public void SendOTPEmail(string toEmail, string otp, string username)
        {
            SendEmail se = new SendEmail();
            se.SendOTPAsync(toEmail, otp, username);
        }
        [HttpGet("SendAccept")]
        public void SendAcceptEmail(string toEmail, string username, string content)
        {
            SendEmail se = new SendEmail();
            se.SendAcceptAsync(toEmail, username, content);
        }
        [HttpGet("SendDecline")]
        public void SendDeclineEmail(string toEmail, string username, string content)
        {
            SendEmail se = new SendEmail();
            se.SendDeclineAsync(toEmail, username, content);
        }
    }
}
