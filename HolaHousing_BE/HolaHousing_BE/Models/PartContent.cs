using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HolaHousing_BE.Models
{
    public partial class PartContent
    {
        public int PartContentId { get; set; }
        public byte? Type { get; set; }
        public string? Src { get; set; }
        public string? Alt { get; set; }
        public string? Content { get; set; }
        public byte? Order { get; set; }
        public int? NewId { get; set; }
        [JsonIgnore]
        public virtual New? New { get; set; }
    }
}
