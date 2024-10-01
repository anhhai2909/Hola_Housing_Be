using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmentitiesController : ControllerBase
    {
        private readonly IAmentityInterface _amentityInterface;
        private readonly IMapper _mapper;
        public AmentitiesController(IAmentityInterface amentityInterface,IMapper mapper)
        {
            _amentityInterface = amentityInterface;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAmentities() {
            var item = _mapper.Map<List<AmentityDTO>>(_amentityInterface.GetAmentities());
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("{id}")]
        public IActionResult GetAmentity(int id)
        {
            var item = _mapper.Map<AmentityDTO>(_amentityInterface.GetAmentity(id));
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("GetAmentitiesByProperty/{id}")]
        public IActionResult GetAmentityByProperty(int id)
        {
            var item = _mapper.Map<List<AmentityDTO>>(_amentityInterface.GetAmentitiesByProperty(id));
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
    }
}
