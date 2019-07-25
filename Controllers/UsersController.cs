using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Player_Service.Models;
using Player_Service.Services;

namespace Player_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService service)
        {
            _userService = service;
        }

        // GET: api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUserItems()
        {
            return _userService.Get();
        }

        // GET api/user/5
        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public IActionResult Get(string id)
        {
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            _userService.Create(user);

            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        // PUT api/user/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Put(string id, User userIn)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Update(id, userIn);

            return NoContent();
        }

        // DELETE api/user/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            _userService.Remove(user.Id);

            return NoContent();
        }
    }
}
