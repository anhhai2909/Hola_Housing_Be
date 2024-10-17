using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HolaHousing_BE.Models
{
    public partial class Amentity
    {
        public Amentity()
        {
            Properties = new HashSet<Property>();
        }

        public int AmentityId { get; set; }
        public string? AmentityName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Property> Properties { get; set; }
    }
}
