﻿using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using HolaHousing_BE.Services.EmailServices;
using HolaHousing_BE.Services.ImageService;
using HolaHousing_BE.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly INotificationInterface _notificationInterface;
        private readonly IPropertyInterface _propertyInterface;
        private readonly IUserInterface _userInterface;
        private readonly IMapper _mapper;
        private readonly NotificationService _notificationService;
        private readonly ImageService _imageService;
        private readonly SendEmail sendEmail;
        public PropertiesController(IPropertyInterface propertyInterface, INotificationInterface notificationInterface, IMapper mapper, IHubContext<NotificationHub> hubContext, IUserInterface userInterface)
        {
            _notificationInterface = notificationInterface;
            _propertyInterface = propertyInterface;
            _mapper = mapper;
            _notificationService = new NotificationService(hubContext);
            _imageService = new ImageService();
            _userInterface = userInterface;
            sendEmail = new SendEmail();
        }

        [HttpGet("GetProsByPosterAndStatus")]
        public IActionResult GetProsByPosterAndStatus([FromQuery] int posterId, [FromQuery] int statusId)
        {
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetPropertiesByPosterAndStatus(posterId, statusId));
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

        [HttpGet]
        public IActionResult GetProperties()
        {
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetProperties());
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

        [HttpGet("Manage")]
        public IActionResult GetPropertiesManage([FromBody] int uid)
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

        [HttpGet("SearchAndFilter")]
        public IActionResult SearchAndFilter([FromQuery] int? sortBy, [FromQuery] String? searchString
            , [FromQuery] String? propertyType, [FromQuery] String? address
            , [FromQuery] String? city, [FromQuery] String? district
            , [FromQuery] String? ward, [FromQuery] decimal? priceFrom
            , [FromQuery] decimal? priceTo, [FromQuery] int pageSize
            , [FromQuery] int pageNumber, [FromQuery] double lat, [FromQuery] double lng)
        {
            int size = 0;
            var properties = _propertyInterface.paging(_mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.SearchProperty(sortBy, searchString
                , propertyType, address
                , city, district
                , ward, priceFrom
                , priceTo, lat, lng)), pageSize, pageNumber, ref size);
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
            return ModelState.IsValid ? Ok(new
            {
                data = properties,
                total = size
            }) : BadRequest(ModelState);
        }
        [HttpGet("{proId}")]
        public IActionResult GetPropertiyByID(int proId, [FromQuery] int userId)
        {
            var property = _mapper.Map<PropertyDTO>(_propertyInterface.GetPropertyByID(proId));
            if (property == null)
            {
                return NotFound();
            }
            if (property.Status == 0 || property.Status == 2)
            {
                var u = _mapper.Map<UserDTO>(_userInterface.GetUser(userId));
                if (u.RoleId == 1)
                {
                    return Ok(property);
                }
                else
                {
                    if (userId == property.PosterId)
                    {
                        return Ok(property);
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
            }
            else
            {
                return ModelState.IsValid ? Ok(property) : BadRequest(ModelState);
            }

        }
        [HttpGet("GetDeclineReasons")]
        public IActionResult GetDeclineReason(int proId)
        {
            var reasons = _propertyInterface.GetReasonsByPro(proId);
            if (reasons == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(reasons) : BadRequest(ModelState);
        }

        [HttpGet("GetPhoneNum/{userId}")]
        public IActionResult GetPhoneNumByID(int userId)
        {
            String phoneNum = _propertyInterface.GetPhone(userId);
            if (String.IsNullOrEmpty(phoneNum))
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(phoneNum) : BadRequest(ModelState);
        }

        [HttpGet("SearchByLatAndLng")]
        public IActionResult SearchByLatAndLng(double lat, double lng, int pid)
        {
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetPropertiesNear(lat, lng, pid, 10000));
            if (properties == null)
            {
                return NotFound();
            }
            foreach (var item in properties)
            {
                foreach (var p in item.PostPrices)
                {
                    if (p.PostPriceId == 1)
                    {
                        item.ManyImg = true;
                        break;
                    }
                    else
                    {
                        item.ManyImg = false;
                    }
                }
            }
            return ModelState.IsValid ? Ok(properties) : BadRequest(ModelState);
        }
        [HttpPost("GetPropertiesByAmentities")]
        public IActionResult GetPropertiyByAmentities(List<int> amentities)
        {
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetPropertiesByAmentities(amentities));
            foreach (var item in properties)
            {
                foreach (var p in item.PostPrices)
                {
                    if (p.PostPriceId == 1)
                    {
                        item.ManyImg = true;
                        break;
                    }
                    else
                    {
                        item.ManyImg = false;
                    }
                }
            }
            return ModelState.IsValid ? Ok(properties) : BadRequest(ModelState);
        }

        [HttpPost("Upload/Image/{pid}")]
        public async Task<IActionResult> UploadImage(int pid, [FromForm] List<IFormFile> images)
        {
            var imagesProperty = new List<PropertyImage>();
            var p = _propertyInterface.GetPropertyByID(pid);
            if (p == null)
            {
                return NotFound("Not found property with id " + pid);
            }
            foreach (var item in images)
            {
                if (item == null || item.Length == 0)
                    return BadRequest("No file provided or the file is empty.");

                try
                {
                    // Call your image upload method with the file and file name
                    var imageUrl = await _imageService.UploadImageAsync1(item, item.FileName);
                    var propertyImage = new PropertyImage
                    {
                        PropertyId = p.PropertyId,
                        Image = imageUrl
                    };
                    imagesProperty.Add(propertyImage);
                }
                catch (Exception ex)
                {
                    // Handle errors
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            if (_propertyInterface.UpdateImages(p.PropertyId, imagesProperty))
            {
                return Ok(new { message = "Images uploaded successfully" });
            }
            else
            {
                return BadRequest(new { error = "Images upload failed" });
            }

        }

        [HttpPost("Create")]
        public IActionResult CreateProperty([FromBody] Property propertyCreate)
        {
            if (propertyCreate == null)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            propertyCreate.PropertyId = 0;

            return Ok(_propertyInterface.CreateProperty(propertyCreate));
        }


        [HttpPost("CreatePropertyDeclineReason")]
        public IActionResult CreateProperTyDeclineReason([FromQuery] int proId, [FromQuery] int? reasonId, [FromQuery] String? others)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_propertyInterface.GetPropertyDeclineReason(proId, reasonId) != null)
            {
                return BadRequest("Existed");
            }
            if (!_propertyInterface.AddPropertyDeclineReason(proId, reasonId, others))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("Update/{userId}")]
        public IActionResult UpdateProperty([FromBody] Property propertyUpdate, int userId)
        {
            if (propertyUpdate == null)
                return BadRequest(ModelState);
            var existingProperty = _propertyInterface.GetPropertyByID(propertyUpdate.PropertyId);

            if (existingProperty == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            if (_mapper.Map<UserDTO>(_userInterface.GetUser(userId)).RoleId == 1)
            {
                return Ok(_propertyInterface.UpdateProperty(propertyUpdate));
            }
            else
            {
                if (userId == propertyUpdate.PosterId)
                {
                    return Ok(_propertyInterface.UpdateProperty(propertyUpdate));
                }
                else
                {
                    return BadRequest("Không có khả năng truy cấp");
                }
            }

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

        [HttpDelete("{propertyId}")]
        public IActionResult DeleteProperty(int propertyId)
        {
            if (!_propertyInterface.IsExisted(propertyId))
            {
                return NotFound();
            }

            var propertyToDelete = _propertyInterface.GetPropertyByID(propertyId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_propertyInterface.DeleteProperty(propertyToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting property");
            }

            return NoContent();
        }
        [HttpDelete("DeleteProDeclineReasonByPro")]
        public IActionResult DeleteProDeclineReasonByPro(int propertyId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_propertyInterface.DeletePropertyDeclineReasons(propertyId))
            {
                ModelState.AddModelError("", "Something went wrong deleting property decline reason");
            }

            return NoContent();
        }
        [HttpGet("GetOwnerProfile/{userId}")]
        public IActionResult GetOwnerProfile(int userId)
        {
            var user = _mapper.Map<UserDTO>(_propertyInterface.GetUserById(userId));
            if (user == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(user) : BadRequest(ModelState);
        }

        [HttpGet("GetPropertiesByPoster/{posterId}")]
        public IActionResult GetPropertiesByPoster(int posterId, [FromQuery] int pid)
        {
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetPropertiesByPoster(posterId, pid));
            if (properties == null)
            {
                return NotFound();
            }
            foreach (var item in properties)
            {
                foreach (var p in item.PostPrices)
                {
                    if (p.PostPriceId == 1)
                    {
                        item.ManyImg = true;
                        break;
                    }
                    else
                    {
                        item.ManyImg = false;
                    }
                }
            }
            return ModelState.IsValid ? Ok(properties) : BadRequest(ModelState);
        }
    }
}
