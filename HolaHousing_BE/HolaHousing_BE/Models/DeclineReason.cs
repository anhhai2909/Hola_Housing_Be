using System;
using System.Collections.Generic;

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

        public virtual ICollection<PropertyDeclineReason> PropertyDeclineReasons { get; set; }
    }
}
