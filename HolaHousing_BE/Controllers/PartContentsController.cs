using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartContentsController : ControllerBase
    {
        private readonly IPartContentInterface _partContentInterface;
        private readonly IMapper _mapper;
        public PartContentsController(IPartContentInterface partContentInterface, IMapper mapper)
        {
            _mapper = mapper;
            _partContentInterface = partContentInterface;   
        }
        [HttpGet]
        public IActionResult GetPartContents() {
            var item = _mapper.Map<List<PartContentDTO>>(_partContentInterface.GetPartContents());
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public IActionResult GetPartContent(int id) { 
            var item = _mapper.Map<PartContentDTO>(_partContentInterface.GetPartContent(id));
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("GetPartContentsByNewId/{newId}")]
        public IActionResult GetPartContentsByNewId(int newId)
        {
            var item = _mapper.Map<List<PartContentDTO>>(_partContentInterface.GetPartContentsByNewId(newId)).OrderBy(p=>p.Order);
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
    }
}
