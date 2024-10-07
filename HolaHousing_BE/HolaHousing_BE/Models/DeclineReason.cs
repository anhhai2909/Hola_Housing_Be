using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HolaHousing_BE.Models
{
    public partial class DeclineReason
    {
        public DeclineReason()
        {
            PropertyDeclineReasons = new HashSet<PropertyDeclineReason>();
        }

        public int ReasonId { get; set; }
        public string? ReasonContent { get; set; }
        [JsonIgnore]
        public virtual ICollection<PropertyDeclineReason> PropertyDeclineReasons { get; set; }
    }
}
