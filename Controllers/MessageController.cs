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
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService service)
        {
            _messageService = service;
        }

        // GET: api/message
        [HttpGet]
        public ActionResult<IEnumerable<Message>> GetMessageItems()
        {
            return _messageService.Get();
        }

        // GET api/message/:id
        [HttpGet("{id:length(24)}", Name = "GetMessage")]
        public IActionResult Get(string id)
        {
            var message = _messageService.Get(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpPost]
        public ActionResult<Message> Create(Message message)
        {
            _messageService.Create(message);

            return CreatedAtRoute("GetMessage", new { id = message.Id.ToString() }, message);
        }

        // PUT api/message/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Put(string id, Message messageIn)
        {
            var message = _messageService.Get(id);

            if (message == null)
            {
                return NotFound();
            }

            _messageService.Update(id, messageIn);

            return NoContent();
        }

        // DELETE api/message/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var message = _messageService.Get(id);

            if (message == null)
            {
                return NotFound();
            }

            _messageService.Remove(message.Id);

            return NoContent();
        }
    }
}
