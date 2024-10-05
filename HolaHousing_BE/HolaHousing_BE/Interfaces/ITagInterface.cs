using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface ITagInterface
    {
        ICollection<Tag> GetTags();
        Tag GetTag(int id);
        bool IsExisted(int id);
        bool SaveChanged();
        bool DeleteTag(Tag tag);
        bool CreateTag(Tag tag);
        bool UpdateTag(Tag tag);
        Tag GetTagByTagName(String name);
        bool validUpdate(Tag tag);
        ICollection<New> GetNewsByTagId(int id);
    }
}
