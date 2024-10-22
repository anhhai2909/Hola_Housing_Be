using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
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

        public RegisterController(IConfiguration configuration, IUserInterface userInterface)
        {
            _configuration = configuration;
            _userInterface = userInterface;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var otp = GenerateOTP();
                User u = new User
                {
                    Fullname = request.Fullname,
                    Email = request.Email,
                    Password = HashPassword(request.Password),
                    PhoneNum = request.PhoneNumber,
                    Status = 2,
                    RoleId = 1
                };
                int id = _userInterface.AddUser(u);
                return Ok(new { message = "OTP đã được gửi đến số điện thoại của bạn", uid = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Đã xảy ra lỗi khi đăng ký", details = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPRequest request)
        {
            try
            {
                User? u = _userInterface.GetUser(request.UserId);
                if(u == null)
                {
                    return StatusCode(404, new { error = "Không tìm thấy user id" + request.UserId});
                }
                else
                {
                    if(u.PhoneNum != request.PhoneNumber)
                    {
                        return StatusCode(404, new { error = "Lỗi xảy ra: Số điện thoại không khớp"});
                    }
                    else
                    {
                        // Xu ly OTP
                        if (true)
                        {
                            u.Status = 1;
                            if(_userInterface.UpdateUser(u.UserId, u))
                            {
                                return Ok(new { message = "Đăng ký thành công" });
                            }
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
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
        public int UserId { get; set; }

    }

    public class OTPData
    {
        public string OTP { get; set; }
        public string CreatedAt { get; set; }
    }
}
