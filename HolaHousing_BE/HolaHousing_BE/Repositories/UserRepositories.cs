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

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            if (_context.SaveChanges() > 0)
            {
                return user.UserId;
            }
            else
            {
                return 0;
            }
        }

        public bool DeleteUser(int id)
        {
            User u = _context.Users.Find(id);
            if (u != null)
            {
                _context.Users.Remove(u);
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public User? GetUser(int id)
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

        public bool UpdateUser(int id, User user)
        {
            User u = _context.Users.Find(id);

            if (u != null)
            {
                u.Fullname = user.Fullname;
                u.PhoneNum = user.PhoneNum;
                u.Email = user.Email;
                u.Status = user.Status;
                u.Password = user.Password;
                u.RoleId = user.RoleId;

                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
