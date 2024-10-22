using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using HolaHousing_BE.Services.EmailServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace UserRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserInterface _userInterface;
        private readonly SendEmail sendEmail;

        public RegisterController(IConfiguration configuration, IUserInterface userInterface)
        {
            _configuration = configuration;
            _userInterface = userInterface;
            sendEmail = new SendEmail();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                User user = _userInterface.GetUserByEmail(request.Email);
                if(user != null)
                {
                    return BadRequest(new { error = "Email đã tồn tại!"});
                }
                var otp = GenerateOTP();
                User u = new User
                {
                    Fullname = request.Fullname,
                    Email = request.Email,
                    Password = HashPassword(request.Password),
                    PhoneNum = request.PhoneNumber,
                    Status = 2,
                    RoleId = 2,
                    Otp = otp
                };

                await sendEmail.SendOTPAsync(request.Email, otp, request.Fullname);

                int id = _userInterface.AddUser(u);
                return Ok(new { message = "OTP đã được gửi đến số điện thoại của bạn", uid = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Đã xảy ra lỗi khi đăng ký", details = ex.Message });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPRequest request)
        {
            try
            {
                User? u = _userInterface.GetUser(request.UserId);
                if (u == null)
                {
                    return StatusCode(404, new { error = "Không tìm thấy user id" + request.UserId });
                }
                else
                {
                    // Xu ly OTP
                    if (request.OTP == u.Otp)
                    {
                        u.Status = 1;
                        u.Otp = "";
                        if (_userInterface.UpdateUser(u.UserId, u))
                        {
                            return Ok(new { message = "Đăng ký thành công" });
                        }
                    }
                }
                return NotFound(new { message = "Xác nhận OTP thất bại." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Đã xảy ra lỗi khi xác thực OTP", details = ex.Message });
            }
        }

        private string GenerateOTP()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }

    // DTOs
    public class RegisterRequest
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class VerifyOTPRequest
    {
        public string OTP { get; set; }
        public int UserId { get; set; }

    }

    public class OTPData
    {
        public string OTP { get; set; }
        public string CreatedAt { get; set; }
    }
}
