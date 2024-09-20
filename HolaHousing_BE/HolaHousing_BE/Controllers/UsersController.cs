using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        private readonly IMapper _mapper;
        public UsersController(IUserInterface userInterface, IMapper mapper)
        {
            _mapper = mapper;
            _userInterface = userInterface;
        }
        [HttpGet]
        public IActionResult GetUsers() { 
            var item = _mapper.Map<List<UserDTO>>(_userInterface.GetUsers());
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(item);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetUser(int id) {
            var item = _mapper.Map<UserDTO>(_userInterface.GetUser(id));
            if (item == null) { 
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            else
            {
                return Ok(item);
            }
        }
    }
}
