using System;
using System.Collections.Generic;

namespace HolaHousing_BE.Models
{
    public partial class PostType
    {
        public PostType()
        {
            PostPrices = new HashSet<PostPrice>();
        }

        public int TypeId { get; set; }
        public string? TypeName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<PostPrice> PostPrices { get; set; }
    }
}
