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
    public class PartContentsController : ControllerBase
    {
        private readonly IPartContentInterface _partContentInterface;
        private readonly INewInterface _newInterface;
        private readonly IMapper _mapper;
        public PartContentsController(IPartContentInterface partContentInterface, IMapper mapper,INewInterface newInterface)
        {
            _mapper = mapper;
            _partContentInterface = partContentInterface;   
            _newInterface = newInterface;
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
        [HttpPost("Create")]
        public IActionResult CreatePartContent([FromBody] PartContentDTO partContentCreate)
        {
            if (partContentCreate == null)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var partContentMap = _mapper.Map<PartContent>(partContentCreate);
            partContentMap.PartContentId = 0;
            if (!_partContentInterface.CreatePartContent(partContentMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("Update")]
        public IActionResult UpdatePartContent([FromBody] PartContentDTO partContentUpdate)
        {
            if (partContentUpdate == null)
                return BadRequest(ModelState);

            var existingPartContent = _partContentInterface.GetPartContent(partContentUpdate.PartContentId);
            if (existingPartContent == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            _mapper.Map(partContentUpdate, existingPartContent);

            if (!_partContentInterface.UpdatePartContent(existingPartContent))
            {
                ModelState.AddModelError("", "Something went wrong updating part content");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{partContentId}")]
        public IActionResult DeletePartContent(int partContentId)
        {
            if (!_partContentInterface.IsExisted(partContentId))
            {
                return NotFound();
            }

            var partContentToDelete = _partContentInterface.GetPartContent(partContentId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_partContentInterface.DeletePartContent(partContentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting partcontent");
            }

            return NoContent();
        }
        [HttpDelete("DeletePCByNewId/{newId}")]
        public IActionResult DeletePartContentByNew(int newId)
        {
            if (!_newInterface.IsExisted(newId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_partContentInterface.DeletePartContentsByNew(newId))
            {
                ModelState.AddModelError("", "Something went wrong deleting partcontent");
            }

            return NoContent();
        }
    }
    
}

