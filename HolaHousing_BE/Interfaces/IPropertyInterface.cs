using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPropertyInterface
    {
        ICollection<Property> GetPreoperties();
        Property GetPropertyByID(int id);
        public string GetFirstImage(int id);
        public bool IsExisted(int id);
    }
}
