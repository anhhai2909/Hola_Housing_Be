using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HolaHousing_BE.Models
{
    public partial class PropertyImage
    {
        public int PropertyId { get; set; }
        public string Image { get; set; } = null!;
        [JsonIgnore]
        public virtual Property? Property { get; set; } = null!;
    }
}
