using SmartHomeController.Key;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Entities;

namespace SmartHomeControllers.Controllers
{
    [ApiController]
    [Route("/Homes/")] // Base route for the HomeController
    [ApiKey]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        // GET /Homes/{uid:long}/Homes
        [HttpGet]
        [Route("{uid:long}")]
        public async Task<ActionResult<List<Home>>>
            GetHomes([FromRoute] long uid)
        {
            try
            {
                // Retrieve and return a list of homes for the user.
                ICollection<Home> homes = await _homeService.RetrieveUserHomes(uid);
                return Ok(homes);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response with status code 404.
                return StatusCode(404, ex.Message);
            }
        }

        // POST /Homes/{uid:long}
        [HttpPost]
        [Route("{uid:long}")]
        public async Task<ActionResult<Home>> AddNewHome([FromRoute] long uid, [FromBody] Home home)
        {
            try
            {
                // Add a new home for the user and return the added home.
                Home newHome = await _homeService.AddNewHome(uid, home);
                return Ok(newHome);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response with status code 500.
                return StatusCode(500, ex.Message);
            }
        }

        // PATCH /Homes
        [HttpPatch]
        public async Task<ActionResult> ModifyHome([FromBody] Home home)
        {
            try
            {
                // Update an existing home and return the modified home.
                Home modifiedHome = await _homeService.ModifyHome(home);
                return Ok(modifiedHome);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response with status code 500.
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE /Homes/{homeId:long}
        [HttpDelete]
        [Route("{homeId:long}")]
        public async Task<ActionResult> DeleteHome([FromRoute] long homeId)
        {
            try
            {
                // Delete a home with the specified ID and return the deleted home.
                Home deletedHome = await _homeService.DeleteHome(homeId);
                return Ok(deletedHome);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response with status code 500.
                return StatusCode(500, ex.Message);
            }
        }

        // GET /Homes/lastMeasurement
        [HttpGet]
        [Route("lastMeasurement")]
        public async Task<ActionResult<Home>> GetLastMeasurementAtHome()
        {
            try
            {
                // Retrieve and return the home with the last measurement.
                Home lastMeasurementHome = await _homeService.GetLastMeasurementAtHome();
                return Ok(lastMeasurementHome);
            }
            catch (Exception e)
            {
                // Handle exceptions and return an error response with status code 500.
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        // GET /Homes/WithLastMeasurements/{uId:long}
        [HttpGet]
        [Route("withLastMeasurements/{uId:long}")]
        public async Task<ActionResult<ICollection<LastMeasurement>>>
            RetrieveHomesWithLastMeasurement([FromRoute] long uId)
        {
            try
            {
                // Retrieve and return homes with their last measurements for a user.
                ICollection<LastMeasurement> homesWithLastMeasurement = await _homeService.RetrieveHomesWithLastMeasurement(uId);
                return Ok(homesWithLastMeasurement);
            }
            catch (Exception e)
            {
                // Handle exceptions and return an error response with status code 404.
                Console.WriteLine(e.Message);
                return StatusCode(404, e.Message);
            }
        }
    }
}
