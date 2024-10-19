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
    }
}
