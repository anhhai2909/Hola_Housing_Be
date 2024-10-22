using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using HolaHousing_BE.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static HolaHousing_BE.Controllers.PropertiesController;

namespace HolaHousing_BE.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeclineReasonsController : ControllerBase
    {
        private readonly IDeclineReasonInterface _declineReasonInterface;
        private readonly IUserInterface _userInterface;
        public DeclineReasonsController(IDeclineReasonInterface declineReasonInterface, IUserInterface userInterface)
        {
            _declineReasonInterface = declineReasonInterface;
            _userInterface = userInterface;
        }
        [HttpGet]
        public IActionResult GetDeclineReasons()
        {
            var item = _declineReasonInterface.GetReasons();
            return ModelState.IsValid ? Ok(item) : BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult UpdateStatus([FromQuery] int pid, [FromBody] UpdateStatusRequest? request)
        {
            if(request == null || request.Reasons.IsNullOrEmpty())
            {
                return NoContent();
            }

            foreach (var item in request.Reasons)
            {
                var i = new PropertyDeclineReason
                {
                    PropertyId = pid,
                    ReasonId = item,
                    Others = item == 4 ? request.OtherReason : ""
                };
                _declineReasonInterface.AddPropertyDeclineReason(i);
            }
            return NoContent();
        }
    }
}

public class UpdateStatusRequest
{
    public List<int>? Reasons { get; set; }
    public string? OtherReason { get; set; }
}