using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPropertyInterface
    {
        ICollection<Property> GetProperties();
        ICollection<Property> GetPropertiesByAmentities(List<int> amentities);
        Property GetPropertyByID(int id);
        public string GetFirstImage(int id);
        public bool IsExisted(int id);
        public IEnumerable<Property> GetPropertiesNear(double latitude, double longitude, double radiusInMeters);
    }
}
