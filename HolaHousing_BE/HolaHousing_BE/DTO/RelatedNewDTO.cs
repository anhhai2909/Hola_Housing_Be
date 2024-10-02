using HolaHousing_BE.Models;

namespace HolaHousing_BE.DTO
{
    public class RelatedNewDTO
    {
        public RelatedNewDTO() { }
        public int NewId { get; set; }
        public int Count { get; set; } = 0;
        public virtual NewDTO? New { get; set; }
    }
}
