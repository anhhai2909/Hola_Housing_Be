using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace HolaHousing_BE.Repositories
{
    public class TagRepositories : ITagInterface
    {
        private readonly EXE201Context _context;
        public TagRepositories(EXE201Context context)
        {
            _context = context;
        }

        public bool CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            return SaveChanged();
        }

        public bool DeleteTag(Tag tag)
        {
            foreach (var item in GetNewsByTagId(tag.TagId)) { 
                _context.News.Remove(item);
            }
            _context.Tags.Remove(tag);
            return SaveChanged();
        }

        public ICollection<New> GetNewsByTagId(int id)
        {
           return _context.Tags.Include(t=>t.News).FirstOrDefault(t=>t.TagId==id).News.ToList();
        }

        public Tag GetTag(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.TagId == id);
        }

        public Tag GetTagByTagName(string name)
        {
            return _context.Tags.FirstOrDefault(t => t.TagName.ToLower().Equals(name.ToLower().Trim()));
        }

        public ICollection<Tag> GetTags()
        {
            return _context.Tags.ToList();
        }

        public bool IsExisted(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.TagId == id) != null ? true : false;
        }

        public bool SaveChanged()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
            return SaveChanged();
        }

        public bool validUpdate(Tag tag)
        {
            return _context.Tags.FirstOrDefault(t=>t.TagId!=tag.TagId&&t.TagName.ToLower().Equals(tag.TagName.ToLower().Trim()))!=null?false:true;
        }
    }
}
