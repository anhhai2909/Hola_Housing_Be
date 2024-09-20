using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IUserInterface
    {
        ICollection<User> GetUsers();
        User GetUser(int id);

        bool IsExisted(int id);
    }
}
