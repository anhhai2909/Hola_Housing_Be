using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface IDeclineReasonInterface
    {
        ICollection<DeclineReason> GetReasons();
        int AddPropertyDeclineReason(PropertyDeclineReason reasons);
    }
}
