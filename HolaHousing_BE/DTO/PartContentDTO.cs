namespace HolaHousing_BE.DTO
{
    public class PartContentDTO
    {
        public int PartContentId { get; set; }
        public byte? Type { get; set; }
        public string? Src { get; set; }
        public string? Alt { get; set; }
        public string? Content { get; set; }
        public byte? Order { get; set; }
        public int? NewId { get; set; }
    }
}
