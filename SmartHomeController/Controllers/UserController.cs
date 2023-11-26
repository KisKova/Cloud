using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SmartHomeController.Key;

namespace SmartHomeControllers.Controllers
{
    [ApiController]
    [Route("/users")]  
    
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get a user by their username.
        /// </summary>
        [HttpGet]
        [Route("by-username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername([FromRoute] string username)
        {
            try
            {
                User user = await _service.GetUserByUsername(username);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get a user by their email.
        /// </summary>
        [HttpGet]
        [Route("by-email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail([FromRoute] string email)
        {
            try
            {
                User user = await _service.GetUserByEmail(email);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get a user by their ID.
        /// </summary>
        [HttpGet]
        [Route("by-id/{id:long}")]
        public async Task<ActionResult<User>> GetUserById([FromRoute] long id)
        {
            try
            {
                User user = await _service.GetUserById(id);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User user)
        {
            try
            {
                User newUser = await _service.RegisterUser(user);
                return Ok(newUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Remove a user by their ID.
        /// </summary>
        [HttpDelete]
        [Route("remove/{id:long}")]
        public async Task<ActionResult<User>> RemoveUser([FromRoute] long id)
        {
            try
            {
                User removedUser = await _service.RemoveUser(id);
                return Ok(removedUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update a user's information.
        /// </summary>
        [HttpPatch]
        [Route("update")]
        public async Task<ActionResult<User>> UpdateUser([FromBody] User user)
        {
            try
            {
                User updatedUser = await _service.UpdateUser(user);
                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Log in a user by email and password.
        /// </summary>
        [HttpGet]
        [Route("login")]
        public async Task<ActionResult<User>> LogUserIn([FromQuery] string email, [FromQuery] string password)
        {
            try
            {
                User loggedInUser = await _service.LogUserIn(email, password);
                return Ok(loggedInUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, e.Message);
            }
        }
    }
}
