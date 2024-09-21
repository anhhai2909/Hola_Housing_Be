using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Repositories
{
    public class TagRepositories : ITagInterface
    {
        private readonly EXE201Context _context;
        public TagRepositories(EXE201Context context)
        {
            _context = context;
        }
        public Tag GetTag(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.TagId == id);
        }

        public ICollection<Tag> GetTags()
        {
            return _context.Tags.ToList();
        }

        public bool IsExisted(int id)
        {
            return _context.Tags.FirstOrDefault(t => t.TagId == id) != null ? true : false;
        }
    }
}
