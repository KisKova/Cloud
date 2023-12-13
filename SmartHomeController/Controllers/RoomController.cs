
using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHomeController.Key;

namespace SmartHomeControllers.Controllers
{
    [ApiController]
    [Route("/roomProfiles/")]
    //[ApiKey]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // Create a new room profile for a user.
        [HttpPost]
        [Route("add/{userId:long}")]
        public async Task<ActionResult<RoomProfile>> CreateRoomProfile([FromRoute] long userId, [FromBody] RoomProfile roomProfile)
        {
            try
            {
                RoomProfile newRoomProfile = await _roomService.CreateRoomProfile(roomProfile, userId);
                return Ok(newRoomProfile);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // Delete a room profile by its ID.
        [HttpDelete]
        [Route("remove/{roomProfileId:long}")]
        public async Task<ActionResult<RoomProfile>> DeleteRoomProfile([FromRoute] long roomProfileId)
        {
            try
            {
                RoomProfile roomProfile = await _roomService.DeleteRoomProfile(roomProfileId);
                return Ok(roomProfile);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // Update an existing room profile.
        [HttpPatch]
        [Route("modify/")]
        public async Task<ActionResult<RoomProfile>> ModifyRoomProfile([FromBody] RoomProfile updatedRoomProfile)
        {
            try
            {
                RoomProfile roomProfile = await _roomService.ModifyRoomProfile(updatedRoomProfile);
                return Ok(roomProfile);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // Get room profiles specific to a user.
        [HttpGet]
        [Route("getUserSpecificRoomProfiles/{userId:long}")]
        public async Task<ActionResult<ICollection<RoomProfile>>>
        GetUserSpecificRoomProfiles([FromRoute] long userId)
        {
            try
            {
                ICollection<RoomProfile> roomProfiles = await _roomService.GetUserSpecificRoomProfiles(userId);
                return Ok(roomProfiles);
            }
            catch (Exception e)
            {
                return StatusCode(505, e.Message);
            }
        }

        // Get default room profiles.
        [HttpGet]
        [Route("defaultRoomProfiles")]
        public async Task<ActionResult<ICollection<RoomProfile>>> GetDefaultRoomProfiles()
        {
            try
            {
                ICollection<RoomProfile> defaultRoomProfiles = await _roomService.GetDefaultRoomProfiles();
                return Ok(defaultRoomProfiles);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // Retrieve a room profile by its ID.
        [HttpGet]
        [Route("retrieveRoomProfileById/{roomProfileId:long}")]
        public async Task<ActionResult<RoomProfile>> RetrieveRoomProfileById([FromRoute] long roomProfileId)
        {
            try
            {
                RoomProfile roomProfile = await _roomService.RetrieveRoomProfileById(roomProfileId);
                return Ok(roomProfile);
            }
            catch (Exception e)
            {
                return StatusCode(404, e.Message);
            }
        }

        // Set a room profile as active for a specific home.
        [HttpPatch]
        [Route("activate/{roomProfileId:long}/{homeId:long}")]
        public async Task<ActionResult> SetRoomProfileAsActive([FromRoute] long roomProfileId, [FromRoute] long homeId)
        {
            try
            {
                await _roomService.SetRoomProfileAsActive(roomProfileId, homeId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // Set a room profile as inactive.
        [HttpPatch]
        [Route("deactivate/{roomProfileId:long}")]
        public async Task<ActionResult> SetRoomProfileAsInactive([FromRoute] long roomProfileId)
        {
            try
            {
                await _roomService.SetRoomProfileAsInactive(roomProfileId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // Get the active room profile for a specific home.
        [HttpGet]
        [Route("activeRoomProfileForHome/{homeId:long}")]
        public async Task<ActionResult<RoomProfile>> GetActiveRoomProfileForHome([FromRoute] long homeId)
        {
            try
            {
                RoomProfile activeRoomProfile = await _roomService.GetActiveRoomProfileForHome(homeId);
                return Ok(activeRoomProfile);
            }
            catch (Exception e)
            {
                return StatusCode(404, e.Message);
            }
        }
    }
}
