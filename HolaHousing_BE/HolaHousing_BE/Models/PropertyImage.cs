using System;
using System.Collections.Generic;

namespace HolaHousing_BE.Models
{
    public partial class PropertyImage
    {
        public int PropertyId { get; set; }
        public string? Image { get; set; }

        public virtual Property Property { get; set; } = null!;
    }
}
