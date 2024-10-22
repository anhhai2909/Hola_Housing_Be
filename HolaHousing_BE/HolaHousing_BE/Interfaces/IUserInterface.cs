using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IUserInterface
    {
        ICollection<User> GetUsers();
        User? GetUser(int id);
        bool IsExisted(int id);
        int AddUser(User user);
        bool UpdateUser(int id, User user);
        bool DeleteUser(int id);
    }
}
