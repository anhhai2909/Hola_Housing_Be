using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Repositories
{
    public class AmentityRepositories : IAmentityInterface
    {
        private readonly EXE201Context _context;
        public AmentityRepositories(EXE201Context context)
        {
            _context = context;
        }

        public bool AddAmentity(Amentity amentity)
        {
            _context.Amentities.Add(amentity);
            return SavedChange();
        }

        public ICollection<Amentity> GetAmentities()
        {
            return _context.Amentities.ToList();
        }

        public ICollection<Amentity> GetAmentitiesByProperty(int propertyId)
        {
            return _context.Amentities.Where(a => a.Properties.FirstOrDefault(p => p.PropertyId == propertyId).PropertyId == propertyId).ToList();
        }

        public Amentity GetAmentity(int id)
        {
            return _context.Amentities.FirstOrDefault(a=>a.AmentityId==id);
        }
        public bool SavedChange()
        {
            return _context.SaveChanges()>0?true:false;
        }

        public bool UpdateAmentity(Amentity entity)
        {
            _context.Amentities.Update(entity);
            return SavedChange();
        }
    }
}
