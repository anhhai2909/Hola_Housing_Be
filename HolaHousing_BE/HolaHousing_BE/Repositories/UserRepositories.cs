using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Repositories
{
    public class UserRepositories : IUserInterface
    {
        private readonly EXE201Context _context;
        public UserRepositories(EXE201Context context)
        {
            _context = context;
        }
        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public bool IsExisted(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id) != null ? true : false;
        }
    }
}
