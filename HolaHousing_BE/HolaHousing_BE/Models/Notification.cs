using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HolaHousing_BE.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Url { get; set; }
        public bool? IsRead { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
