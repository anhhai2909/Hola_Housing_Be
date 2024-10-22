using HolaHousing_BE.Models;

namespace HolaHousing_BE.DTO
{
    public class SmallPropertyDTO
    {
        public int PropertyId { get; set; }
        public string? Content { get; set; }
        public decimal? Price { get; set; }
        public DateTime? PostTime { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Address { get; set; }
        public string? PropertyType { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public double? Area { get; set; }
        public string? Owner { get; set; }
        public int? Status { get; set; }
        public bool? ManyImg { get; set; } = false;
        public int? PosterId { get; set; }
        public virtual PostPrice? PostPrice { get; set; }
        public virtual ICollection<PropertyImageDTO>? PropertyImages { get; set; } = new List<PropertyImageDTO>();
        public virtual ICollection<PostPriceDTO>? PostPrices { get; set; }
    }
}
