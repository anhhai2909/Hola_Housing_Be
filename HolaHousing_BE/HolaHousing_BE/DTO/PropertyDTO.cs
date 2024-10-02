using HolaHousing_BE.Models;

namespace HolaHousing_BE.DTO
{
    public class PropertyDTO
    {
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
        public int? PostPriceId { get; set; }
        public string? Image {  get; set; }
        public virtual ICollection<PropertyImageDTO> PropertyImages { get; set; }
        public virtual ICollection<AmentityDTO> Amentities { get; set; }
    }
}
