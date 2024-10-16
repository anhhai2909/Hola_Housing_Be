using System;
using System.Collections.Generic;

namespace HolaHousing_BE.Models
{
    public partial class PropertyDeclineReason
    {
        public int PropertyId { get; set; }
        public int ReasonId { get; set; }
        public string? Others { get; set; }

        public virtual Property Property { get; set; } = null!;
        public virtual DeclineReason Reason { get; set; } = null!;
    }
}
