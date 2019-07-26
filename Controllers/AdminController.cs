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
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService service)
        {
            _adminService = service;
        }

        // GET: api/admin
        [HttpGet]
        public ActionResult<IEnumerable<Admin>> GetUserItems()
        {
            return _adminService.Get();
        }

        // GET api/admin/5
        [HttpGet("{id:length(24)}", Name = "GetAdmin")]
        public IActionResult Get(string id)
        {
            var admin = _adminService.Get(id);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin);
        }

        [HttpPost]
        public ActionResult<Admin> Create(Admin admin)
        {
            _adminService.Create(admin);

            return CreatedAtRoute("GetAdmin", new { id = admin.Id.ToString() }, admin);
        }

        // PUT api/admin/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Put(string id, Admin adminIn)
        {
            var admin = _adminService.Get(id);

            if (admin == null)
            {
                return NotFound();
            }

            _adminService.Update(id, adminIn);

            return NoContent();
        }

        // DELETE api/admin/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var admin = _adminService.Get(id);

            if (admin == null)
            {
                return NotFound();
            }

            _adminService.Remove(admin.Id);

            return NoContent();
        }
    }
}
