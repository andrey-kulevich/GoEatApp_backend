using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoEatapp_backend;
using GoEatapp_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GoEatApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        // Get dialogs preview list by user id
        [HttpGet("dialogs/{userId}")]
        public async Task<ActionResult> Get(int userId)
        {
            List<Dialog> dialogs = new List<Dialog>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getDialogsList({userId});", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Dialog dialog;
                    while (reader.Read())
                    {
                        dialog = new Dialog();
                        dialog.PersonId = reader.GetInt32(0);
                        dialog.PersonName = DBUtils.SafeGetString(reader, 1);
                        dialog.Avatar = DBUtils.SafeGetString(reader, 2);
                        dialogs.Add(dialog);
                    }
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(dialogs));
        }

        // Get messages by user id and recipient id
        [HttpGet("{userId}/{recipientId}")]
        public async Task<ActionResult> GetMessagesInDialog(int userId, int recipientId)
        {
            List<Message> messages = new List<Message>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"SELECT * FROM message " +
                $"WHERE (sender = {userId} AND recipient = {recipientId}) OR (sender = {recipientId} AND recipient = {userId});", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Message message;
                    while (reader.Read())
                    {
                        message = new Message();
                        message.Id = reader.GetInt32(0);
                        message.DateTime = DBUtils.SafeGetString(reader, 1);
                        message.Content = DBUtils.SafeGetString(reader, 2);
                        message.SenderId = reader.GetInt32(3);
                        message.RecipientId = reader.GetInt32(4);
                        messages.Add(message);
                    }
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(messages));
        }

        // Create message
        [HttpPost]
        public ActionResult Post(MessageCreate message)
        {
            if (message == null || message.DateTime == null || message.Content == null) return BadRequest();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO message (datetime, content, sender, recipient)" +
                    $"VALUES('{message.DateTime}', '{message.Content}', {message.SenderId}, {message.RecipientId});", conn);
            int affected = cmd.ExecuteNonQuery();
            if (affected == 0) return NotFound();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return Ok();
        }
    }
}
