using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Repositories
{
    public class PartContentRepositories : IPartContentInterface
    {
        private readonly EXE201Context _context;
        public PartContentRepositories(EXE201Context context)
        {
            _context = context;
        }
        public PartContent GetPartContent(int id)
        {
            return _context.PartContents.FirstOrDefault(p => p.PartContentId == id);
        }

        public ICollection<PartContent> GetPartContents()
        {
            return _context.PartContents.ToList();
        }

        public ICollection<PartContent> GetPartContentsByNewId(int newId)
        {
            return _context.PartContents.Where(p => p.NewId == newId).ToList();
        }

        public bool IsExisted(int id)
        {
            return _context.PartContents.FirstOrDefault(p => p.PartContentId == id) != null ? true : false;
        }
    }
}
