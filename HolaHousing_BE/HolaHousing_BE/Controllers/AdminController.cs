using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using HolaHousing_BE.Services.EmailServices;
using HolaHousing_BE.Services.ImageService;
using HolaHousing_BE.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HolaHousing_BE.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly INotificationInterface _notificationInterface;
        private readonly IPropertyInterface _propertyInterface;
        private readonly IUserInterface _userInterface;
        private readonly IMapper _mapper;
        private readonly NotificationService _notificationService;
        private readonly SendEmail sendEmail;
        public AdminController(IPropertyInterface propertyInterface, INotificationInterface notificationInterface, IMapper mapper, IHubContext<NotificationHub> hubContext, IUserInterface userInterface)
        {
            _notificationInterface = notificationInterface;
            _propertyInterface = propertyInterface;
            _mapper = mapper;
            _notificationService = new NotificationService(hubContext);
            _userInterface = userInterface;
            sendEmail = new SendEmail();
        }

        [HttpGet("Manage")]
        public IActionResult GetPropertiesManage([FromQuery] int uid)
        {
            User u = _userInterface.GetUser(uid);
            if (u == null || u.RoleId != 1) return StatusCode(403);
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetPropertiesManage(2));
            foreach (var item in properties)
            {
                foreach (var p in item.PostPrices)
                {
                    if (p.PostPriceId == 1)
                    {
                        item.ManyImg = true;
                        break;
                    }
                }
            }
            return ModelState.IsValid ? Ok(properties) : BadRequest(ModelState);
        }

        [HttpPut("UpdateStatus")]
        public IActionResult UpdateStatus([FromQuery] int propertyId, [FromQuery] int status, [FromBody] int uid)
        {
            User us = _userInterface.GetUser(uid);
            if (us == null || us.RoleId != 1) return StatusCode(403);

            if (propertyId == null || status == null)
                return BadRequest(ModelState);

            Property p = _propertyInterface.GetPropertyByID(propertyId);
            if (p == null)
            {
                return NotFound("Not found property");
            }

            User u = _userInterface.GetUser(p.PosterId.Value);

            if (_propertyInterface.GetPropertyByID(propertyId) == null)
            {
                return NotFound();
            }
            if (_mapper.Map<UserDTO>(_userInterface.GetUser(uid)).RoleId == 1)
            {
                if (!_propertyInterface.UpdateStatus(propertyId, status))
                {
                    ModelState.AddModelError("", "Something went wrong updating status");
                    return StatusCode(500, ModelState);
                }
            }
            else
            {
                if (uid == _mapper.Map<PropertyDTO>(_propertyInterface.GetPropertyByID(propertyId)).PosterId)
                {
                    if (!_propertyInterface.UpdateStatus(propertyId, status))
                    {
                        ModelState.AddModelError("", "Something went wrong updating status");
                        return StatusCode(500, ModelState);
                    }
                }
                else
                {
                    return BadRequest("Không có khả năng truy cấp");
                }
            }

            if (!_propertyInterface.UpdateStatus(propertyId, status))
            {
                ModelState.AddModelError("", "Something went wrong updating status");
                return StatusCode(500, ModelState);
            }

            if (status == 0)
            {
                Notification n = new Notification
                {
                    Title = "Từ chối tin đăng",
                    Description = "Tin đăng của bạn đã bị từ chối, xem thông báo để biết chi tiết",
                    CreatedDate = DateTime.Now,
                    Url = "/detail/" + propertyId,
                    IsRead = false,
                    UserId = u.UserId
                };
                sendEmail.SendDeclineAsync(u.Email, u.Fullname, "Tin đăng của bạn đã bị từ chối, truy cập website Hola Housing để biết chi tiết");
                _notificationInterface.AddNotification(n);
                _notificationService.SendNotification(n, u.UserId);
            }
            else if (status == 1)
            {
                Notification n = new Notification
                {
                    Title = "Tin đăng được đăng tải thành công",
                    Description = "Tin đăng của bạn đã được duyệt thành công",
                    CreatedDate = DateTime.Now,
                    Url = "/detail/" + propertyId,
                    IsRead = false,
                    UserId = u.UserId
                };
                sendEmail.SendAcceptAsync(u.Email, u.Fullname, $"Tin đăng của bạn về nhà trọ \"{p.Content}\" đã được duyệt thành công, truy cập website Hola Housing để biết chi tiết");
                _notificationInterface.AddNotification(n);
                _notificationService.SendNotification(n, u.UserId);
            }
            return NoContent();
        }
    }
}
