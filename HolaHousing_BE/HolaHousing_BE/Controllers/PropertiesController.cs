using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenAnhHai_Assignment1_PRN231.AutoMapper;

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
            var properties = _mapper.Map<List<PropertyDTO>>(_propertyInterface.GetPreoperties());
            foreach (var p in properties) {
                if (_propertyInterface.GetPropertyByID(p.PropertyId).PropertyImages.Count > 0)
                {
                    p.Image = _propertyInterface.GetFirstImage(p.PropertyId);
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(properties);
            }
        }
    }
}
