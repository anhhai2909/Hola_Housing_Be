using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using HolaHousing_BE.Services.ImageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HolaHousing_BE.Controllers
{
    [Authorize(Roles = "user, admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        private readonly IPropertyInterface _propertyInterface;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;
        public UsersController(IUserInterface userInterface, IMapper mapper, IPropertyInterface propertyInterface)
        {
            _mapper = mapper;
            _userInterface = userInterface;
            _propertyInterface = propertyInterface;
            _imageService = new ImageService();
        }
        [HttpGet]
        public IActionResult GetUsers() { 
            var item = _mapper.Map<List<UserDTO>>(_userInterface.GetUsers());
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("{id}")]
        public IActionResult GetUser(int id) {
            var item = _mapper.Map<UserDTO>(_userInterface.GetUser(id));
            if (item == null) { 
                return NotFound();
            }
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }

        [HttpGet("phone/{pid}")]
        public IActionResult GetPropertyPhoneNumber(int pid)
        {
            var item = _propertyInterface.GetPhoneByProperty(pid);
            if (String.IsNullOrEmpty(item))
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }

        [HttpPost("Create")]
        public IActionResult CreateProperty([FromBody] Property propertyCreate)
        {
            if (propertyCreate == null)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            propertyCreate.PropertyId = 0;

            return Ok(_propertyInterface.CreateProperty(propertyCreate));
        }

        [HttpPost("Upload/Image/{pid}")]
        public async Task<IActionResult> UploadImage(int pid, [FromForm] List<IFormFile> images)
        {
            var imagesProperty = new List<PropertyImage>();
            var p = _propertyInterface.GetPropertyByID(pid);
            if (p == null)
            {
                return NotFound("Not found property with id " + pid);
            }
            foreach (var item in images)
            {
                if (item == null || item.Length == 0)
                    return BadRequest("No file provided or the file is empty.");

                try
                {
                    // Call your image upload method with the file and file name
                    var imageUrl = await _imageService.UploadImageAsync1(item, item.FileName);
                    var propertyImage = new PropertyImage
                    {
                        PropertyId = p.PropertyId,
                        Image = imageUrl
                    };
                    imagesProperty.Add(propertyImage);
                }
                catch (Exception ex)
                {
                    // Handle errors
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            if (_propertyInterface.UpdateImages(p.PropertyId, imagesProperty))
            {
                return Ok(new { message = "Images uploaded successfully" });
            }
            else
            {
                return BadRequest(new { error = "Images upload failed" });
            }

        }
    }
}
