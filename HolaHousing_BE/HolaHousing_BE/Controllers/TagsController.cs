using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagInterface _tagInterface;
        private readonly IMapper _mapper;
        public TagsController(ITagInterface tagInterface, IMapper mapper)
        {
            _mapper = mapper;
            _tagInterface = tagInterface;
        }
        [HttpGet]
        public IActionResult GetTags() { 
            var item = _mapper.Map<List<TagDTO>>(_tagInterface.GetTags());
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("{id}")]
        public IActionResult GetTag(int id) {
            var item = _mapper.Map<TagDTO>(_tagInterface.GetTag(id));
            if(item == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }


    }
}
