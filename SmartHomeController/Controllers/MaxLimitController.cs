using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SmartHomeController.Key;
using Microsoft.AspNetCore.Mvc;
namespace SmartHomeController.Controllers
{
    [ApiController]
    [Route("/MaxLimits/")]
    //[ApiKey]
    public class MaxLimitController : ControllerBase
    {
        private IMaxLimitService _service;

        public MaxLimitController(IMaxLimitService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get the maximum threshold limits for a specific room profile.
        /// </summary>
        [HttpGet]
        [Route("roomProfileThreshold/{roomProfileId:long}")]
        public async Task<ActionResult<ThresholdLimits>> GetThresholdForRoomProfile([FromRoute] long roomProfileId)
        {
            try
            {
                ThresholdLimits thresholdLimits = await _service.FetchThresholdForSpecificRoomProfile(roomProfileId);
                return Ok(thresholdLimits);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update the maximum threshold limits for a specific room profile.
        /// </summary>
        [HttpPatch]
        [Route("updateRoomProfileThreshold/{roomProfileId:long}")]
        public async Task<ActionResult> UpdateThresholdForRoomProfile([FromBody] ThresholdLimits updatedThresholdLimits, [FromRoute] long roomProfileId)
        {
            try
            {
                await _service.UpdateThresholdForRoomProfile(updatedThresholdLimits, roomProfileId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Retrieve the maximum threshold limits for the current room of a home.
        /// </summary>
        [HttpGet]
        [Route("currentRoomThreshold/{homeId:long}")]
        public async Task<ActionResult<ThresholdLimits>> RetrieveThresholdForCurrentRoom([FromRoute] long homeId)
        {
            try
            {
                ThresholdLimits thresholdLimits = await _service.RetrieveThresholdForCurrentRoom(homeId);
                return Ok(thresholdLimits);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
