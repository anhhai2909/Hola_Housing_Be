using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeclineReasonsController : ControllerBase
    {
        private readonly IDeclineReasonInterface _declineReasonInterface;
        public DeclineReasonsController(IDeclineReasonInterface declineReasonInterface)
        {
            _declineReasonInterface = declineReasonInterface;
        }
        [HttpGet]
        public IActionResult GetDeclineReasons()
        {
            var item = _declineReasonInterface.GetReasons();
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }
    }
}
