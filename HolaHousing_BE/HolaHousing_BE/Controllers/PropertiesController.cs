using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using HolaHousing_BE.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly INotificationInterface _notificationInterface;
        private readonly IPropertyInterface _propertyInterface;
        private readonly IMapper _mapper;
        private readonly NotificationService _notificationService;
        public PropertiesController(IPropertyInterface propertyInterface, INotificationInterface notificationInterface, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _notificationInterface = notificationInterface;
            _propertyInterface = propertyInterface;
            _mapper = mapper;
            _notificationService = new NotificationService(hubContext);
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
        [HttpGet("SearchAndFilter")]
        public IActionResult SearchAndFilter([FromQuery] int? sortBy, [FromQuery] String? searchString
            , [FromQuery] String? propertyType, [FromQuery] String? address
            , [FromQuery] String? city, [FromQuery] String? district
            , [FromQuery] String? ward, [FromQuery] decimal? priceFrom
            , [FromQuery] decimal? priceTo, [FromQuery] int pageSize
            , [FromQuery] int pageNumber)
        {
            var properties = _propertyInterface.paging(_mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.SearchProperty(sortBy, searchString
                , propertyType, address
                , city, district
                , ward, priceFrom
                , priceTo)), pageSize, pageNumber);
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
        [HttpGet("{id}")]
        public IActionResult GetPropertiyByID(int id)
        {
            var property = _mapper.Map<PropertyDTO>(_propertyInterface.GetPropertyByID(id));
            if (property == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(property) : BadRequest(ModelState);
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
        public IActionResult SearchByLatAndLng(double lat, double lng)
        {
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetPropertiesNear(lat, lng, 10000));
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
        [HttpPost("Create")]
        public IActionResult CreateProperty([FromBody] PropertyDTO propertyCreate)
        {
            if (propertyCreate == null)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var propertyMap = _mapper.Map<Property>(propertyCreate);
            propertyMap.PropertyId = 0;
            if (!_propertyInterface.CreateProperty(propertyMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
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

        [HttpPut("Update/{propertyId}")]
        public IActionResult UpdateProperty(int propertyId, [FromBody] PropertyDTO propertyUpdate)
        {
            if (propertyUpdate == null)
                return BadRequest(ModelState);

            if (propertyId != propertyUpdate.PropertyId)
                return BadRequest(ModelState);

            var existingProperty = _propertyInterface.GetPropertyByID(propertyId);
            if (existingProperty == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            _mapper.Map(propertyUpdate, existingProperty);

            if (!_propertyInterface.UpdateProperty(existingProperty))
            {
                ModelState.AddModelError("", "Something went wrong updating property");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpPut("UpdateStatus")]
        public IActionResult UpdateStatus([FromQuery] int propertyId, [FromQuery] int status)
        {
            if (propertyId == null || status == null)
                return BadRequest(ModelState);

            if (_propertyInterface.GetPropertyByID(propertyId) == null)
            {
                return NotFound();
            }
            ;

            if (!_propertyInterface.UpdateStatus(propertyId, status))
            {
                ModelState.AddModelError("", "Something went wrong updating status");
                return StatusCode(500, ModelState);
            }
            int userId = 1;
            if (status == 0)
            {
                Notification n = new Notification
                {
                    Title = "Từ chối tin đăng",
                    Description = "Tin đăng của bạn đã bị từ chối, xem thông báo để biết chi tiết",
                    CreatedDate = DateTime.Now,
                    Url = "/list/",
                    IsRead = false,
                    UserId = userId
                };
                _notificationInterface.AddNotification(n);
                _notificationService.SendNotification(n, userId);
            }
            else if (status == 1)
            {
                Notification n = new Notification
                {
                    Title = "Tin đăng được đăng tải thành công",
                    Description = "Tin đăng của bạn đã được duyệt thành công",
                    CreatedDate = DateTime.Now,
                    Url = "/detail/5",
                    IsRead = false,
                    UserId = userId
                };
                _notificationInterface.AddNotification(n);
                _notificationService.SendNotification(n, userId);
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
        public IActionResult GetPropertiesByPoster(int posterId)
        {
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetPropertiesByPoster(posterId));
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

        //public async Task<string> UploadImage(IFormFile file, string name)
        //{
        //    string clientId = "f260b84e886a6c3";
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(memoryStream);
        //        byte[] fileBytes = memoryStream.ToArray();

        //        using (var client = new HttpClient())
        //        {
        //            // Thêm Client ID của bạn vào header Authorization
        //            client.DefaultRequestHeaders.Add("Authorization", "Client-ID " + clientId);

        //            // Tạo nội dung multipart form để gửi file lên
        //            var content = new MultipartFormDataContent();
        //            content.Add(new ByteArrayContent(fileBytes), "image", name);

        //            // Gửi yêu cầu POST lên Imgur API
        //            var response = await client.PostAsync("https://api.imgur.com/3/upload", content);
        //            var responseData = await response.Content.ReadAsStringAsync();
        //            var imageUrl = JsonConvert.DeserializeObject<ImgurResponseData>(responseData);
        //            return imageUrl.Data.Link; // Kết quả trả về chứa URL của ảnh đã upload
        //        }
        //    }
        //}
    }
}

public class ImgurResponseData
{
    public ImgurImageDataViewModel Data { get; set; }
    public bool Success { get; set; }
    public int Status { get; set; }
}

public class ImgurImageDataViewModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Datetime { get; set; }
    public string Type { get; set; }
    public bool Animated { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Size { get; set; }
    public int Views { get; set; }
    public int Bandwidth { get; set; }
    public string Ddeletehash { get; set; }
    public string Section { get; set; }
    public string Link { get; set; }
    public string Account_url { get; set; }
    public int Aaccount_id { get; set; }
}
