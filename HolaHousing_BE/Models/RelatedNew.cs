namespace HolaHousing_BE.Models
{
    public class RelatedNew
    {
        public RelatedNew() { }
        public int NewId { get; set; }
        public int Count { get; set; } = 0;
        public virtual New? New { get; set; }
    }
}
