using HolaHousing_BE.DTO;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPropertyInterface
    {
        ICollection<Property> GetProperties();
        ICollection<Property> GetPropertiesByAmentities(List<int> amentities);
        ICollection<Property> GetPropertiesByPoster(int posterId, int pid);
        ICollection<SmallPropertyDTO> paging(List<SmallPropertyDTO> list,int pageSize,int pageNumber);
        Property GetPropertyByID(int id);
        public string GetFirstImage(int id);
        public bool IsExisted(int id);
        public IEnumerable<Property> GetPropertiesNear(double latitude, double longitude, int pid, double radiusInMeters);
        String GetPhone(int userId);
        int CreateProperty(Property property);
        int UpdateProperty(Property property);
        bool DeleteProperty(Property property);
        User GetUserById(int id);
        ICollection<Property> GetPropertiesByPosterAndStatus(int userId, int status);
        PropertyDeclineReason GetPropertyDeclineReason(int proId,int? reasonId);
        bool AddPropertyDeclineReason(int proId, int? reasonId, String others);
        bool DeletePropertyDeclineReasons(int proId);
        ICollection<PropertyDeclineReason> GetReasonsByPro(int proId);
        bool UpdateStatus(int propertyId, int status);
        ICollection<Property> SearchProperty(int? sortBy, String? searchString
            , String? propertyType, String? address
            , String? city, String? district
            , String? ward, decimal? priceFrom
            , decimal? priceTo);
        bool SaveChanged();
    }
}
