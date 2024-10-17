using System;
using System.Collections.Generic;

namespace HolaHousing_BE.Models
{
    public partial class Property
    {
        public Property()
        {
            PropertyDeclineReasons = new HashSet<PropertyDeclineReason>();
            PropertyImages = new HashSet<PropertyImage>();
            Amentities = new HashSet<Amentity>();
            PostPrices = new HashSet<PostPrice>();
        }

        public int PropertyId { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public DateTime? PostTime { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? PropertyType { get; set; }
        public double? Area { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string? PhoneNum { get; set; }
        public string? Owner { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PosterId { get; set; }

        public virtual User? Poster { get; set; }
        public virtual ICollection<PropertyDeclineReason>? PropertyDeclineReasons { get; set; }
        public virtual ICollection<PropertyImage>? PropertyImages { get; set; }

        public virtual ICollection<Amentity>? Amentities { get; set; }
        public virtual ICollection<PostPrice>? PostPrices { get; set; }
    }
}
