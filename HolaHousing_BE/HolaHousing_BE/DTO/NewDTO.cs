namespace HolaHousing_BE.DTO
{
    public class NewDTO
    {
        public int NewId { get; set; }
        public string? Title { get; set; }
        public string? Sumary { get; set; }
        public string? Author { get; set; }
        public DateTime? PostDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
