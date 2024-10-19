using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPartContentInterface
    {
        ICollection<PartContent> GetPartContents();
        PartContent GetPartContent(int id);
        ICollection<PartContent> GetPartContentsByNewId(int newId);
        bool IsExisted(int id);
    }
}
