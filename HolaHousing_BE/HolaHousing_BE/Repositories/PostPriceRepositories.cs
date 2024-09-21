using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Repositories
{
    public class PostPriceRepositories : IPostPriceInterface
    {
        private readonly EXE201Context _context;
        public PostPriceRepositories(EXE201Context context)
        {
            _context = context;
        }
        public PostPrice GetPostPrice(int id)
        {
            return _context.PostPrices.FirstOrDefault(p => p.PostPriceId == id);
        }

        public ICollection<PostPrice> GetPostPrices()
        {
            return _context.PostPrices.ToList();
        }

        public List<PostPrice> GetPostPricesByTypeId(int id)
        {
            return _context.PostPrices.Where(p=>p.TypeId==id).ToList();
        }

        public bool IsExisted(int id)
        {
            return _context.PostPrices.FirstOrDefault(p => p.PostPriceId == id) != null ? true : false;
        }
    }
}
