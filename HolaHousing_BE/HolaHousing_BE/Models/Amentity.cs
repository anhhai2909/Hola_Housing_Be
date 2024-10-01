using System;
using System.Collections.Generic;

namespace HolaHousing_BE.Models
{
    public partial class Amentity
    {
        public Amentity()
        {
            Properties = new HashSet<Property>();
        }

        public int AmentityId { get; set; }
        public string? Amentity_Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
