using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HolaHousing_BE.Models
{
    public partial class Tag
    {
        public Tag()
        {
            News = new HashSet<New>();
        }

        public int TagId { get; set; }
        public string? TagName { get; set; }
        [JsonIgnore]
        public virtual ICollection<New> News { get; set; }
    }
}
