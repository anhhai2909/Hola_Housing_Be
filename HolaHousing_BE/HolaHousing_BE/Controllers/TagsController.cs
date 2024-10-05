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

        [HttpGet("GetNewsByTagId/{id}")]
        public IActionResult GetNewsByTagId(int id)
        {
            var item = _mapper.Map<List<NewDTO>>(_tagInterface.GetNewsByTagId(id));
            if (item == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpPost("Create")]
        public IActionResult CreateTag([FromBody] TagDTO tagCreate)
        {
            if (tagCreate == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_tagInterface.GetTagByTagName(tagCreate.TagName) != null)
            {
                return BadRequest("Tag name existed");
            }
            var tagMap = _mapper.Map<Models.Tag>(tagCreate);
            tagMap.TagId = 0;
            
            if (!_tagInterface.CreateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("Update")]
        public IActionResult UpdateProperty([FromBody] TagDTO tagUpdate)
        {
            if (tagUpdate == null)
                return BadRequest(ModelState);

            var existingTag = _tagInterface.GetTag(tagUpdate.TagId);
            if (existingTag == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();
            
            _mapper.Map(tagUpdate, existingTag);
            if (!_tagInterface.validUpdate(existingTag))
            {
                return BadRequest("Tag name existed");
            }
            if (!_tagInterface.UpdateTag(existingTag))
            {
                ModelState.AddModelError("", "Something went wrong updating tag");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{tagId}")]
        public IActionResult DeleteTag(int tagId)
        {
            if (!_tagInterface.IsExisted(tagId))
            {
                return NotFound();
            }

            var tagToDelete = _tagInterface.GetTag(tagId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_tagInterface.DeleteTag(tagToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting property");
            }

            return NoContent();
        }
    }
}
