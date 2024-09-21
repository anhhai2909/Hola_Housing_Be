using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IPostPriceInterface
    {
        ICollection<PostPrice> GetPostPrices();
        PostPrice GetPostPrice(int id);
        List<PostPrice> GetPostPricesByTypeId(int id);
        bool IsExisted(int id);
    }
}
