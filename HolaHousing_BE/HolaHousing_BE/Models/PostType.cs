using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual ICollection<PostPrice> PostPrices { get; set; }
    }
}
