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
        [HttpPost("Create")]
        public IActionResult CreateAmentity([FromBody] AmentityDTO amentityCreate)
        {
            if (amentityCreate == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_amentityInterface.IsExisted(amentityCreate.AmentityName))
                return BadRequest(ModelState);
            var amentityMap = _mapper.Map<Amentity>(amentityCreate);
            amentityMap.AmentityId = 0;
            if (!_amentityInterface.AddAmentity(amentityMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("Update")]
        public IActionResult UpdateAmentity( [FromBody] AmentityDTO amentityUpdate)
        {
            if (amentityUpdate == null)
                return BadRequest(ModelState);
            
            var existingAmentity= _amentityInterface.GetAmentity(amentityUpdate.AmentityId);
            if (existingAmentity == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            _mapper.Map(amentityUpdate, existingAmentity);
            if (!_amentityInterface.ValidToUpdate(existingAmentity))
            {
                return BadRequest("Amentity name existed!");
            }
            if (!_amentityInterface.UpdateAmentity(existingAmentity))
            {
                ModelState.AddModelError("", "Something went wrong updating amentity");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{amentityId}")]
        public IActionResult DeleteAmentity(int amentityId)
        {
            if (_amentityInterface.GetAmentity(amentityId)==null)
            {
                return NotFound();
            }

            var amentityToDelete = _amentityInterface.GetAmentity(amentityId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_amentityInterface.DeleteAmentity(amentityToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting amentity");
            }

            return NoContent();
        }
    }
}
