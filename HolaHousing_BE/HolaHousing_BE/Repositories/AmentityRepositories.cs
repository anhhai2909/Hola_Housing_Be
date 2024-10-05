using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.EntityFrameworkCore;

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

        public bool DeleteAmentity(Amentity amentity)
        {
            var am = _context.Amentities.Include(a => a.Properties).FirstOrDefault(a => a.AmentityId == amentity.AmentityId);
            foreach (var item in am.Properties)
            {
                am.Properties.Remove(item);
            }        
            _context.Amentities.Remove(amentity);
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
            return _context.Amentities.FirstOrDefault(a => a.AmentityId == id);
        }

        public bool IsExisted(string amentityName)
        {
            return _context.Amentities
                .FirstOrDefault(a => a.Amentity_Name
                                .ToLower()
                                .Equals(amentityName.ToLower().Trim())) != null ? true : false;
        }

        public bool SavedChange()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateAmentity(Amentity entity)
        {
            _context.Amentities.Update(entity);
            return SavedChange();
        }

        public bool ValidToUpdate(Amentity entity)
        {
            return _context.Amentities
                .Where(a => a.AmentityId != entity.AmentityId 
                            && a.Amentity_Name
                            .ToLower()
                            .Equals(entity.Amentity_Name
                                    .ToLower().Trim()))
                .ToList().Count() == 0 ? true : false;
        }
    }
}
