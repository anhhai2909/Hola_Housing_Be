using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPropertyInterface
    {
        ICollection<Property> GetProperties();
        ICollection<Property> GetPropertiesByAmentities(List<int> amentities);
        ICollection<Property> GetPropertiesByPoster(int posterId);
        ICollection<Property> paging(int pageSize,int pageNumber);
        Property GetPropertyByID(int id);
        public string GetFirstImage(int id);
        public bool IsExisted(int id);
        public IEnumerable<Property> GetPropertiesNear(double latitude, double longitude, double radiusInMeters);
        String GetPhone(int userId);
        bool CreateProperty(Property property);
        bool UpdateProperty(Property property);
        bool DeleteProperty(Property property);
        User GetUserById(int id);
        bool SaveChanged();
    }
}
