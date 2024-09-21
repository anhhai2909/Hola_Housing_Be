using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostPricesController : ControllerBase
    {
        private readonly IPostPriceInterface _postPriceInterface;
        private readonly IMapper _mapper;
        public PostPricesController(IPostPriceInterface postPriceInterface,IMapper mapper)
        {
            _mapper = mapper;
            _postPriceInterface = postPriceInterface;
        }
        [HttpGet("{id}")]
        public IActionResult GetPostPriceByID(int id) { 
            var item = _mapper.Map<PostPriceDTO>(_postPriceInterface.GetPostPrice(id));
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult GetPostPrices()
        {
            var item = _mapper.Map<List<PostPriceDTO>>(_postPriceInterface.GetPostPrices());
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }

        [HttpGet("GetPostPriceByTypeId/{typeId}")]
        public IActionResult GetPostPriceByTypeId(int typeId) {
            var item = _mapper.Map<List<PostPriceDTO>>(_postPriceInterface.GetPostPricesByTypeId(typeId));
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
    }
}
