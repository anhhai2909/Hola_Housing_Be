using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HolaHousing_BE.Models
{
    public partial class PropertyDeclineReason
    {
        public int PropertyId { get; set; }
        public int? ReasonId { get; set; }
        public string? Others { get; set; }
        [JsonIgnore]
        public virtual Property Property { get; set; } = null!;
        public virtual DeclineReason Reason { get; set; } = null!;
    }
}
