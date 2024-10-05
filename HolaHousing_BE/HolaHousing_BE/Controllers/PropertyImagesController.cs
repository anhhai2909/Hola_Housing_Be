using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImagesController : ControllerBase
    {
        private readonly IPropertyImageInterface _propertyImageInterface;
        private readonly IMapper _mapper;
        public PropertyImagesController(IPropertyImageInterface propertyImageInterface, IMapper mapper)
        {
            _mapper = mapper;
            _propertyImageInterface = propertyImageInterface;
        }
        [HttpGet("{id}")]
        public IActionResult GetPropertyImagesByProID(int id) {
            var list = _mapper.Map<List<PropertyImageDTO>>(_propertyImageInterface.GetPropertyImagesByPropertyID(id));
            return ModelState.IsValid ? Ok(list) : BadRequest(ModelState);
        }
        [HttpPost("Create")]
        public IActionResult CreatePropertyImage([FromBody] PropertyImageDTO propertyImageCreate)
        {
            if (propertyImageCreate == null)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var propertyImageMap = _mapper.Map<PropertyImage>(propertyImageCreate);
            if (!_propertyImageInterface.CreatePropertyImage(propertyImageMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpDelete("DeletePropertyImage")]
        public IActionResult DeletePropertyImage([FromQuery] int propertyId, [FromQuery] String image)
        {
            PropertyImage pi = new PropertyImage();
            pi.PropertyId = propertyId;
            pi.Image = image;
            
            if (!_propertyImageInterface.IsExisted(pi))
            {
                return NotFound();
            }
            var existingPropertyImage = _propertyImageInterface.GetPropertyImage(pi);
            _mapper.Map(pi, existingPropertyImage);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_propertyImageInterface.DeletePropertyImage(existingPropertyImage))
            {
                ModelState.AddModelError("", "Something went wrong deleting property image");
            }

            return NoContent();
        }
    }
}
