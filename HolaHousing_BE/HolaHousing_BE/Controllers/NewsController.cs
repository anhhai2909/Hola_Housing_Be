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
    public class NewsController : ControllerBase
    {
        private readonly INewInterface _newInterface;
        private readonly ITagInterface _tagInterface;
        private readonly IMapper _mapper;
        public NewsController(INewInterface newInterface, ITagInterface tagInterface, IMapper mapper)
        {
            _mapper = mapper;
            _newInterface = newInterface;
            _tagInterface = tagInterface;
        }
        [HttpGet]
        public IActionResult GetNews()
        {
            var item = _mapper.Map<List<NewDTO>>(_newInterface.GetNews());
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }

        [HttpGet("test")]
        public IActionResult Helloworld()
        {
            return Ok("Hello world!");
        }
        [HttpGet("{id}")]
        public IActionResult GetNew(int id)
        {
            var item = _mapper.Map<NewDTO>(_newInterface.GetNew(id));
            if (item == null)
            {
                return NotFound();
            }
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
        [HttpGet("GetTagsByNewId/{newId}")]
        public IActionResult GetTagsByNewId(int newId)
        {
            var item = _mapper.Map<List<TagDTO>>(_newInterface.GetTagsByNewId(newId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(item);
            }
        }

        [HttpGet("GetRelatedNews/{id}")]
        public IActionResult GetRelatedNews(int id)
        {
            var currentNewTags = _newInterface.GetTagsByNewId(id);
            List<RelatedNewDTO> relatedNews = new List<RelatedNewDTO>();
            foreach (var item in _newInterface.GetNews().Where(n => n.NewId != id))
            {
                if (item.Tags.Count > 0)
                {
                    relatedNews.Add(new RelatedNewDTO { NewId = item.NewId, Count = 0 });
                }
            }

            foreach (var item in relatedNews)
            {
                foreach (var t in _newInterface.GetTagsByNewId(item.NewId))
                {
                    foreach (var t2 in currentNewTags)
                    {
                        if (t.TagId == t2.TagId)
                        {
                            item.Count++;
                        }
                    }
                }
            }
            relatedNews.RemoveAll(r => r.Count == 0);

            switch (relatedNews.Count())
            {
                case 0:
                    foreach (var item in _newInterface.GetRandomNews(id, 3))
                    {
                        relatedNews.Add(new RelatedNewDTO { NewId = item.NewId, Count = 0 });
                    }
                    break;
                case 1:
                    foreach (var item in _newInterface.GetRandomNews(id, 2))
                    {
                        relatedNews.Add(new RelatedNewDTO { NewId = item.NewId, Count = 0 });
                    }
                    break;
                case 2:
                    foreach (var item in _newInterface.GetRandomNews(id, 1))
                    {
                        relatedNews.Add(new RelatedNewDTO { NewId = item.NewId, Count = 0 });
                    }
                    break;
                default:
                    relatedNews = relatedNews.OrderByDescending(r => r.Count).Take(3).ToList(); ;
                    break;
            }
            return Ok(relatedNews);
        }
        [HttpPost("Create")]
        public IActionResult CreateNew([FromBody] NewDTO newCreate)
        {
            if (newCreate == null)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newMap = _mapper.Map<New>(newCreate);
            newMap.NewId = 0;
            if (!_newInterface.CreateNew(newMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("Update")]
        public IActionResult UpdateNew([FromBody] NewDTO newUpdate)
        {
            if (newUpdate == null)
                return BadRequest(ModelState);


            var existingNew = _newInterface.GetNew(newUpdate.NewId);
            if (existingNew == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            _mapper.Map(newUpdate, existingNew);

            if (!_newInterface.UpdateNew(existingNew))
            {
                ModelState.AddModelError("", "Something went wrong updating new");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{newId}")]
        public IActionResult DeleteNew(int newId)
        {
            if (!_newInterface.IsExisted(newId))
            {
                return NotFound();
            }

            var newToDelete = _newInterface.GetNew(newId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_newInterface.DeleteNew(newToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting new");
            }

            return NoContent();
        }
    }
}
