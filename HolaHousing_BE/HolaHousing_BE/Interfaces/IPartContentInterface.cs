using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPartContentInterface
    {
        ICollection<PartContent> GetPartContents();
        PartContent GetPartContent(int id);
        ICollection<PartContent> GetPartContentsByNewId(int newId);
        bool DeletePartContentsByNew(int newId);
        bool DeletePartContent(PartContent partContent);
        bool CreatePartContent(PartContent content);
        bool UpdatePartContent(PartContent content);
        bool SaveChanged();
        bool IsExisted(int id);
    }
}
