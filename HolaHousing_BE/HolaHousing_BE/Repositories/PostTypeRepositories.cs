using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Repositories
{
    public class PostTypeRepositories : IPostTypeInterface
    {
        private readonly EXE201Context _context;
        public PostTypeRepositories(EXE201Context context)
        {
                _context = context;
        }
        public PostType GetPostType(int id)
        {
            return _context.PostTypes.FirstOrDefault(p => p.TypeId == id);
        }

        public ICollection<PostType> GetPostTypes()
        {
            return _context.PostTypes.ToList();
        }
    }
}
