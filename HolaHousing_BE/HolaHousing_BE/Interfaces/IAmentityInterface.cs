using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IAmentityInterface
    {
        ICollection<Amentity> GetAmentities();
        Amentity GetAmentity(int id);
        ICollection<Amentity> GetAmentitiesByProperty(int propertyId);
        bool UpdateAmentity(Amentity entity);
        bool AddAmentity(Amentity amentity);
        bool ValidToUpdate(Amentity entity);
        bool IsExisted(String amentityName);
        bool DeleteAmentity(Amentity amentity);
        bool SavedChange();
    }
}
