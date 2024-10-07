using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenAnhHai_Assignment1_PRN231.AutoMapper;
using System.Collections.Generic;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyInterface _propertyInterface;
        private readonly IMapper _mapper;
        public PropertiesController(IPropertyInterface propertyInterface,IMapper mapper)
        {
            _propertyInterface = propertyInterface;
            _mapper = mapper;
        }

        [HttpGet("GetProsByPosterAndStatus")]
        public IActionResult GetProsByPosterAndStatus([FromQuery] int posterId, [FromQuery] int statusId)
        {
            var properties = _mapper.Map<List<SmallPropertyDTO>>(_propertyInterface.GetPropertiesByPosterAndStatus(posterId,statusId));
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
        public IActionResult GetProperties() {            
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
        public IActionResult GetPropertiyByID(int id) { 
            var property = _mapper.Map<PropertyDTO>(_propertyInterface.GetPropertyByID(id));
            if(property == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(property) : BadRequest(ModelState);
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
        public IActionResult SearchByLatAndLng(double lat,double lng)
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

            if(_propertyInterface.GetPropertyByID(propertyId) == null)
            {
                return NotFound();
            }
            ;

            if (!_propertyInterface.UpdateStatus(propertyId, status))
            {
                ModelState.AddModelError("", "Something went wrong updating status");
                return StatusCode(500, ModelState);
            }
            if(status == 0)
            {
                Console.WriteLine("property has been declined");
            }else if(status == 1)
            {
                Console.WriteLine("property has been approved");
            }
            return NoContent();
        }
        [HttpDelete("{propertyId}")]
        public IActionResult DeleteProperty(int propertyId) {
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
        public IActionResult GetPropertiesByPoster(int posterId) {
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
    }
}
