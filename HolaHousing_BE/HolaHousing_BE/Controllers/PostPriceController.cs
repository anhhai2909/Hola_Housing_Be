using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostPriceController : ControllerBase
    {
        private readonly IPostPriceInterface _postPriceInterface;
        private readonly IMapper _mapper;
        public PostPriceController(IPostPriceInterface postPriceInterface,IMapper mapper)
        {
            _mapper = mapper;
            _postPriceInterface = postPriceInterface;
        }
        [HttpGet("{id}")]
        public IActionResult GetPostPriceByID(int id) { 
            var item = _mapper.Map<PostPriceDTO>(_postPriceInterface.GetPostPrice(id));
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(item);
            }
        }
        [HttpGet]
        public IActionResult GetPostPrices()
        {
            var item = _mapper.Map<List<PostPriceDTO>>(_postPriceInterface.GetPostPrices());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(item);
            }
        }
    }
}
