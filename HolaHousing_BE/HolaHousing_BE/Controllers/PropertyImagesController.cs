using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
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
            if (list == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(list) : BadRequest(ModelState);
        }
    }
}
