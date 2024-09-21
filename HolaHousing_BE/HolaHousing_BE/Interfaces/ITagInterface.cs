using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface ITagInterface
    {
        ICollection<Tag> GetTags();
        Tag GetTag(int id);
        bool IsExisted(int id);
    }
}
