using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface INewInterface
    {
        ICollection<New> GetNews();
        New GetNew(int id);
        bool IsExisted(int id);
    }
}
