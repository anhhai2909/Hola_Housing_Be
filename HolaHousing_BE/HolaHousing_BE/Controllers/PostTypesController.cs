using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypesController : ControllerBase
    {
        private readonly IPostTypeInterface _postTypeInterface;
        private readonly IMapper _mapper;
        public PostTypesController(IPostTypeInterface postTypeInterface,IMapper mapper)
        {
            _mapper = mapper;
            _postTypeInterface = postTypeInterface;
        }
        [HttpGet]
        public IActionResult GetPostTypes() { 
            var item = _mapper.Map<List<PostTypeDTO>>(_postTypeInterface.GetPostTypes());
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("{id}")]
        public IActionResult GetPostType(int id)
        {
            var item = _mapper.Map<PostTypeDTO>(_postTypeInterface.GetPostType(id));
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
    }
}
