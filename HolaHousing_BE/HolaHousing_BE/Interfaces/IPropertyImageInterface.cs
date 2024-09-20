using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPropertyImageInterface
    {
        ICollection<PropertyImage> GetPropertyImages();
        ICollection<PropertyImage> GetPropertyImagesByPropertyID(int propertyID);
    }
}
