﻿using HolaHousing_BE.DTO;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPropertyInterface
    {
        ICollection<Property> GetProperties();
        ICollection<Property> GetPropertiesByAmentities(List<int> amentities);
        ICollection<Property> GetPropertiesByPoster(int posterId);
        ICollection<SmallPropertyDTO> paging(List<SmallPropertyDTO> list,int pageSize,int pageNumber);
        Property GetPropertyByID(int id);
        public string GetFirstImage(int id);
        public bool IsExisted(int id);
        public IEnumerable<Property> GetPropertiesNear(double latitude, double longitude, double radiusInMeters);
        String GetPhone(int userId);
        bool CreateProperty(Property property);
        bool UpdateProperty(Property property);
        bool DeleteProperty(Property property);
        User GetUserById(int id);
        ICollection<Property> SearchProperty(int? sortBy, String? searchString
            , String? propertyType, String? address
            , String? city, String? district
            , String? ward, decimal? priceFrom
            , decimal? priceTo);
        bool SaveChanged();
    }
}
