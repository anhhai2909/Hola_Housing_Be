using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewInterface _newInterface;
        private readonly IMapper _mapper;
        public NewsController(INewInterface newInterface, IMapper mapper)
        {
            _mapper = mapper;
            _newInterface = newInterface;
        }
        [HttpGet]
        public IActionResult GetNews() { 
            var item = _mapper.Map<List<NewDTO>>(_newInterface.GetNews());
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("{id}")]
        public IActionResult GetNew(int id) { 
            var item = _mapper.Map<NewDTO>(_newInterface.GetNew(id));
            if (item == null) {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("GetTagsByNewId/{newId}")]
        public IActionResult GetTagsByNewId(int newId) {
            var item = _mapper.Map<List<TagDTO>>(_newInterface.GetTagsByNewId(newId));
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(item);
            }
        }
    }
}
