namespace HolaHousing_BE.DTO
{
    public class PostPriceDTO
    {
        public int PostPriceId { get; set; }
        public int? Duration { get; set; }
        public decimal? Price { get; set; }
        public int? TypeId { get; set; }
    }
}
