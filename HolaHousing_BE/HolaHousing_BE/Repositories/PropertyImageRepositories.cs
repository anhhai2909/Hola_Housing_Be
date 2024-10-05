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

        public bool CreatePropertyImage(PropertyImage propertyImage)
        {
            _context.PropertyImages.Add(propertyImage);
            return SaveChanged();
        }

        public bool DeletePropertyImage(PropertyImage propertyImage)
        {
            _context.PropertyImages.Remove(propertyImage);
            return SaveChanged();
        }

        public PropertyImage GetPropertyImage(PropertyImage propertyImage)
        {
            return _context.PropertyImages.FirstOrDefault(p => p.PropertyId == propertyImage.PropertyId && p.Image.Equals(propertyImage.Image));
        }

        public ICollection<PropertyImage> GetPropertyImages()
        {
            return _context.PropertyImages.ToList();
        }

        public ICollection<PropertyImage> GetPropertyImagesByPropertyID(int propertyID)
        {
            return _context.PropertyImages.Where(p=>p.PropertyId==propertyID).ToList();
        }

        public bool IsExisted(PropertyImage propertyImage)
        {
            return _context.PropertyImages.FirstOrDefault(p => p.PropertyId == propertyImage.PropertyId && p.Image.Equals(propertyImage.Image)) != null ? true : false;
        }

        public bool SaveChanged()
        {
            return _context.SaveChanges()>0?true:false;
        }
    }
}
