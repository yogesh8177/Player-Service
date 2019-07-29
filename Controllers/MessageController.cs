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
        private readonly OneSignalService _notificationService;
        private readonly SendBirdService _chatService;
        private readonly  UserService _userService;

        public MessageController(
            MessageService service, 
            OneSignalService notificationService, 
            SendBirdService chatService,
            UserService userService
        )
        {
            _messageService = service;
            _notificationService = notificationService;
            _chatService = chatService;
            _userService = userService;

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
        public async Task<ActionResult<Message>> CreateAsync(Message message)
        {
            _messageService.Create(message);
            List<User> users = await _userService.GetUsersViaConditionsAsync(message.QueryConditions);
            Console.WriteLine("total users" + users.Count);
            if (users.Count > 0) {
                    List<string> oneSignalIds = new List<string>();
                    foreach (var u in users) {
                        var sendBirdObject = u.Integrations[0].AsBsonDocument;
                        var oneSignalObject = u.Integrations[1].AsBsonDocument;
        
                        string oneSignalId = oneSignalObject.Elements.First().Value.ToString();
                        string channelUrl = sendBirdObject.Elements.Last().Value.ToString();
                        string userAccessToken = sendBirdObject.Elements.First().Value.ToString();
                        
                        oneSignalIds.Add(oneSignalId);
                        if (message.Chat) {
                            await _chatService.sendSystemChatMessageAsync(channelUrl, message.Text);
                        }
                    }
                    if (message.Notification) {
                        var result = _notificationService.createNotificationAsync(oneSignalIds, message.Text);
                        Console.Write("notification result " + result);
                    } 
            }
            else {
                // no users that exists in the given criteria
                // maybe we can schedule a job based on message id at a lateral point to deliver this message.
            } 
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
