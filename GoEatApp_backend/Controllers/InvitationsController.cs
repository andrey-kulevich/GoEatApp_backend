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
    public class InvitationsController : ControllerBase
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        // Get invitation by id
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            Invitation invitation = new Invitation();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getInvitationById({id});", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    PutInvitation(reader, ref invitation);
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(invitation));
        }


        // Get invitations by location
        [HttpGet("{requestArea}/{userLocation}")]
        public async Task<ActionResult> Get(string requestArea, string userLocation)
        {
            List<Invitation> invitations = new List<Invitation>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getInvitationsListByLocation('{requestArea}', '{userLocation}');", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Invitation invitation;
                    while (reader.Read())
                    {
                        invitation = new Invitation();
                        PutInvitation(reader, ref invitation);
                        invitations.Add(invitation);
                    }
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(invitations));
        }

        // Get invitations made by person
        [HttpGet("sender/{personId}")]
        public async Task<ActionResult> GetInvitationsListMadeByPerson(int personId)
        {
            List<Invitation> invitations = new List<Invitation>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getInvitationsListMadeByPerson({personId});", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Invitation invitation;
                    while (reader.Read())
                    {
                        invitation = new Invitation();
                        PutInvitation(reader, ref invitation);
                        invitations.Add(invitation);
                    }
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(invitations));
        }

        // Get personal invitations list 
        [HttpGet("recipient/{personId}")]
        public async Task<ActionResult> GetPersonalInvitations(int personId)
        {
            List<Invitation> invitations = new List<Invitation>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getPersonalInvitationsList({personId});", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Invitation invitation;
                    while (reader.Read())
                    {
                        invitation = new Invitation();
                        PutInvitation(reader, ref invitation);
                        invitations.Add(invitation);
                    }
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(invitations));
        }

        // Create new invitation
        [HttpPost]
        public ActionResult Post(InvitationCreate invitation)
        {
            if (invitation == null || invitation.DateTime == null || invitation.Address == 0 ||
                invitation.Message == null || invitation.SenderId == 0) return BadRequest();
            conn.Open();

            MySqlCommand cmd;
            if (invitation.RecipientId == -1)
            {
                cmd = new MySqlCommand("insert into invitation " +
                                "(datetime, address, who_will_pay, message, inviting_person, accepted) " +
                                $"values ('{invitation.DateTime}', {invitation.Address}, {invitation.WhoWillPay}, '{invitation.Message}', " +
                                $"{invitation.SenderId}, 0);", conn);
            } else
            {
                cmd = new MySqlCommand("insert into invitation " +
                                "(datetime, address, who_will_pay, message, inviting_person, recipient, accepted) " +
                                $"values ('{invitation.DateTime}', {invitation.Address}, {invitation.WhoWillPay}, '{invitation.Message}', " +
                                $"{invitation.SenderId}, {invitation.RecipientId}, 0);", conn);
            }
            
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return new JsonResult("Success!") { StatusCode = StatusCodes.Status201Created };
        }

        // Accept invitation
        [HttpPut]
        public ActionResult Put([FromBody] int invitationId)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"update invitation set accepted = 1 where id = {invitationId};", conn);
            int affected = cmd.ExecuteNonQuery();
            if (affected == 0) return NotFound();
            conn.Close();
            conn.Dispose();
            return Ok();
        }

        // Delete invitation
        [HttpDelete("{invitationId}")]
        public ActionResult Delete(int invitationId)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"delete from invitation where id = {invitationId};", conn);
            int affected = cmd.ExecuteNonQuery();
            if (affected == 0) return NotFound();
            conn.Close();
            conn.Dispose();
            return Ok();
        }

        private void PutInvitation(MySqlDataReader reader, ref Invitation invitation)
        {
            invitation.Id = reader.GetInt32(0);
            invitation.DateTime = DBUtils.SafeGetString(reader, 1);

            Place place = new Place
            {
                Id = reader.GetInt32(2),
                Name = DBUtils.SafeGetString(reader, 3),
                Photo = DBUtils.SafeGetString(reader, 4),
                Country = DBUtils.SafeGetString(reader, 5),
                Region = DBUtils.SafeGetString(reader, 6),
                Town = DBUtils.SafeGetString(reader, 7),
                MailIndex = DBUtils.SafeGetString(reader, 8),
                Street = DBUtils.SafeGetString(reader, 9),
                House = DBUtils.SafeGetString(reader, 10),
                Apartment = DBUtils.SafeGetString(reader, 11),
                CuisineNationality = DBUtils.SafeGetString(reader, 12),
                Interior = DBUtils.SafeGetString(reader, 13),
                Tagline = DBUtils.SafeGetString(reader, 14),
                Other = DBUtils.SafeGetString(reader, 15)
            };

            invitation.Place = place;
            invitation.WhoWillPay = reader.GetInt32(16);
            invitation.Message = DBUtils.SafeGetString(reader, 17);
            invitation.SenderId = reader.GetInt32(18);
            invitation.Sender = DBUtils.SafeGetString(reader, 19);
            if (DBUtils.SafeGetIntId(reader, 20) != -1)
            {
                invitation.RecipientId = reader.GetInt32(20);
                invitation.Recipient = DBUtils.SafeGetString(reader, 21);
            }
            invitation.Accepted = reader.GetInt32(22) != 0;
        }
    }
}
