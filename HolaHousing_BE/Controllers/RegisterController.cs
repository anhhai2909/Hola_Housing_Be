using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System;
using System.Threading.Tasks;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        // Replace with your Twilio account details
        private readonly string accountSid = "your_twilio_account_sid";
        private readonly string authToken = "your_twilio_auth_token";
        private readonly string fromPhoneNumber = "your_twilio_phone_number";

        public RegisterController()
        {
            TwilioClient.Init(accountSid, authToken);
        }

        // POST: api/Register/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Generate OTP
                var otpCode = new Random().Next(100000, 999999).ToString();

                // Send OTP using Twilio
                var message = await MessageResource.CreateAsync(
                    body: $"Your verification code is {otpCode}",
                    from: new PhoneNumber(fromPhoneNumber),
                    to: new PhoneNumber(request.PhoneNumber)
                );

                // Store OTP and other details in session or database for future use
                // For example, save OTP to database along with the user details

                // Return the verification ID or some unique reference for tracking
                return Ok(new { message = "OTP sent to phone number." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error sending OTP: " + ex.Message });
            }
        }

        // POST: api/Register/verifyOtp
        [HttpPost("verifyOtp")]
        public IActionResult VerifyOtp([FromBody] OtpVerificationRequest request)
        {
            try
            {
                // Here you should retrieve the OTP from your database/session and verify
                // This is just a simple example. Replace with actual verification.

                // Assuming you stored OTP in your database
                string storedOtp = "storedOtpFromDatabase";  // Fetch from database

                if (storedOtp == request.OtpCode)
                {
                    // Proceed with user registration or login
                    return Ok(new { message = "Registration successful, redirecting to home." });
                }
                else
                {
                    return BadRequest(new { message = "Invalid OTP. Please try again." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error verifying OTP: " + ex.Message });
            }
        }

        // POST: api/Register/resendOtp
        [HttpPost("resendOtp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequest request)
        {
            try
            {
                // Generate and resend OTP using Twilio
                var otpCode = new Random().Next(100000, 999999).ToString();

                var message = await MessageResource.CreateAsync(
                    body: $"Your verification code is {otpCode}",
                    from: new PhoneNumber(fromPhoneNumber),
                    to: new PhoneNumber(request.PhoneNumber)
                );

                // Update the stored OTP for this user

                return Ok(new { message = "OTP resent." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error resending OTP: " + ex.Message });
            }
        }
    }

    // DTOs for the API
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class OtpVerificationRequest
    {
        public string OtpCode { get; set; }
    }

    public class ResendOtpRequest
    {
        public string PhoneNumber { get; set; }
    }
}
