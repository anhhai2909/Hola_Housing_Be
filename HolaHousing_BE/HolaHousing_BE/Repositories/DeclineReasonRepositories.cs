using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;

namespace HolaHousing_BE.Repositories
{
    public class DeclineReasonRepositories : IDeclineReasonInterface
    {
        private readonly EXE201Context _context;
        public DeclineReasonRepositories(EXE201Context context)
        {
            _context = context;
        }
        public ICollection<DeclineReason> GetReasons()
        {
            return _context.DeclineReasons.ToList();
        }

        public int AddPropertyDeclineReason(PropertyDeclineReason reasons)
        {
            _context.PropertyDeclineReasons.Add(reasons);
            if (_context.SaveChanges() > 0)
            {
                return reasons.ReasonId.GetValueOrDefault();
            }
            else
            {
                return -1;
            }

        }
    }
}
