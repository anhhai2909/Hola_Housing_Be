﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HolaHousing_BE.Models
{
    public partial class PostPrice
    {
        public PostPrice()
        {
            Properties = new HashSet<Property>();
        }

        public int PostPriceId { get; set; }
        public int? Duration { get; set; }
        public decimal? Price { get; set; }
        public int? TypeId { get; set; }

        public virtual PostType? Type { get; set; }
        [JsonIgnore]
        public virtual ICollection<Property>? Properties { get; set; }
    }
}
