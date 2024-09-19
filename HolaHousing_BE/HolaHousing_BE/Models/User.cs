using System;
using System.Collections.Generic;

namespace HolaHousing_BE.Models
{
    public partial class User
    {
        public User()
        {
            News = new HashSet<New>();
            Properties = new HashSet<Property>();
        }

        public int UserId { get; set; }
        public string? Fullname { get; set; }
        public string? PhoneNum { get; set; }
        public string? Email { get; set; }
        public byte? Status { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<New> News { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
    }
}
