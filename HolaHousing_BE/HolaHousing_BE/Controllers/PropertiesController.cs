using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
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
        [HttpGet]
        public IActionResult GetProperties() { 
            var properties = _mapper.Map<List<PropertyDTO>>(_propertyInterface.GetProperties());
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

        [HttpGet("SearchByLatAndLng")]
        public IActionResult SearchByLatAndLng(double lat,double lng)
        {
            var properties = _mapper.Map<List<PropertyDTO>>(_propertyInterface.GetPropertiesNear(lat, lng, 10000));
            if (properties == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(properties) : BadRequest(ModelState);
        }
        [HttpPost("GetPropertiesByAmentities")]
        public IActionResult GetPropertiyByAmentities(List<int> amentities)
        {
            var properties = _mapper.Map<List<PropertyDTO>>(_propertyInterface.GetPropertiesByAmentities(amentities));
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
    }
}
