using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPropertyImageInterface
    {
        ICollection<PropertyImage> GetPropertyImages();
        ICollection<PropertyImage> GetPropertyImagesByPropertyID(int propertyID);
        PropertyImage GetPropertyImage(PropertyImage propertyImage);
        bool DeletePropertyImage(PropertyImage propertyImage);
        bool CreatePropertyImage(PropertyImage propertyImage);
        bool IsExisted(PropertyImage propertyImage);
        bool SaveChanged();
    }
}
