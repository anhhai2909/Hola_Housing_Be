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

        public bool CreatePartContent(PartContent content)
        {
            _context.PartContents.Add(content);
            return SaveChanged();
        }

        public bool DeletePartContent(PartContent partContent)
        {
            _context.PartContents.Remove(partContent);
            return SaveChanged();
        }

        public bool DeletePartContentsByNew(int newId)
        {
            foreach(var item in GetPartContentsByNewId(newId))
            {
                _context.PartContents.Remove(item);
            }
            return SaveChanged();
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

        public bool SaveChanged()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        public bool UpdatePartContent(PartContent content)
        {
            _context.PartContents.Update(content);
            return SaveChanged();
        }
    }
}
