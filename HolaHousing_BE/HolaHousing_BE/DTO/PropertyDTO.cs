using HolaHousing_BE.Models;

namespace HolaHousing_BE.DTO
{
    public class PropertyDTO
    {
        public int PropertyId { get; set; }
        public string? Content { get; set; }
        public decimal? Price { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public double? Area { get; set; }
        public string? Image {  get; set; }
    }
}
