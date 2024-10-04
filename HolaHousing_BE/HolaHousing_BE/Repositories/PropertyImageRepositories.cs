using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Repositories
{
    public class PropertyImageRepositories : IPropertyImageInterface
    {
        private readonly EXE201Context _context;
        public PropertyImageRepositories(EXE201Context context)
        {
            _context = context;
        }

        public bool DeletePropertyImage(PropertyImage propertyImage)
        {
            _context.PropertyImages.Remove(propertyImage);
            return SaveChanged();
        }

        public ICollection<PropertyImage> GetPropertyImages()
        {
            return _context.PropertyImages.ToList();
        }

        public ICollection<PropertyImage> GetPropertyImagesByPropertyID(int propertyID)
        {
            return _context.PropertyImages.Where(p=>p.PropertyId==propertyID).ToList();
        }

        public bool SaveChanged()
        {
            return _context.SaveChanges()>0?true:false;
        }
    }
}
