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
            foreach (var p in properties) {
                if (_propertyInterface.GetPropertyByID(p.PropertyId).PropertyImages.Count > 0)
                {
                    p.Image = _propertyInterface.GetFirstImage(p.PropertyId);
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
    }
}
