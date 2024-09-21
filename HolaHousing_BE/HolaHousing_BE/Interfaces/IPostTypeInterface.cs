using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPostTypeInterface
    {
        ICollection<PostType> GetPostTypes();
        PostType GetPostType(int id);

    }
}
