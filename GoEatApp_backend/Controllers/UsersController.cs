using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GoEatapp_backend;
using MySql.Data.MySqlClient;
using GoEatapp_backend.Models;
using System.Data.Common;
using Microsoft.AspNetCore.Cors;

namespace GoEatApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        // Check auth data
        [HttpGet("{login}/{password}")]
        public async Task<ActionResult> Get(string login, string password)
        {
            conn.Open();
            User user = new User();
            int preferencesId = 0;
            MySqlCommand cmd = new MySqlCommand("call getUserByLoginAndPassword(@login, @password);", conn);
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@password", password);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    user.Id = reader.GetInt32(0);
                    user.Name = DBUtils.SafeGetString(reader, 1);
                    user.Age = reader.GetInt32(2);
                    user.Gender = DBUtils.SafeGetString(reader, 3);
                    user.Avatar = DBUtils.SafeGetString(reader, 4);
                    preferencesId = reader.GetInt32(5);
                    user.Status = DBUtils.SafeGetString(reader, 6);
                    user.Role = DBUtils.SafeGetString(reader, 7);
                }
                else return await Task.FromResult(NotFound());
            }
            user.Preferences = PreferencesController.GetPreferencesByUserId(user.Id, conn);
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(user));
        }

        // Get user by id
        [HttpGet("{userId}")]
        public async Task<ActionResult> Get(int userId)
        {
            conn.Open();
            User user = new User();
            int preferencesId = 0;
            MySqlCommand cmd = new MySqlCommand("call getUserById(@userId);", conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    user.Id = reader.GetInt32(0);
                    user.Name = DBUtils.SafeGetString(reader, 1);
                    user.Age = reader.GetInt32(2);
                    user.Gender = DBUtils.SafeGetString(reader, 3);
                    user.Avatar = DBUtils.SafeGetString(reader, 4);
                    preferencesId = reader.GetInt32(5);
                    user.Status = DBUtils.SafeGetString(reader, 6);
                    user.Role = DBUtils.SafeGetString(reader, 7);
                }
                else return await Task.FromResult(NotFound());
            }
            user.Preferences = PreferencesController.GetPreferencesByUserId(user.Id, conn);
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(user));
        }

        // Get user by login
        [HttpGet("login/{login}")]
        public async Task<ActionResult> GetByLogin(string login)
        {
            conn.Open();
            User user = new User();
            int preferencesId = 0;
            MySqlCommand cmd = new MySqlCommand("call getUserByLogin(@login);", conn);
            cmd.Parameters.AddWithValue("@login", login);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    user.Id = reader.GetInt32(0);
                    user.Name = DBUtils.SafeGetString(reader, 1);
                    user.Age = reader.GetInt32(2);
                    user.Gender = DBUtils.SafeGetString(reader, 3);
                    user.Avatar = DBUtils.SafeGetString(reader, 4);
                    preferencesId = reader.GetInt32(5);
                    user.Status = DBUtils.SafeGetString(reader, 6);
                    user.Role = DBUtils.SafeGetString(reader, 7);
                }
                else return await Task.FromResult(NotFound());
            }
            user.Preferences = PreferencesController.GetPreferencesByUserId(user.Id, conn);
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(user));
        }

        // Create user
        [HttpPost]
        public ActionResult Post(UserCreate user)
        {
            if (user == null) return BadRequest();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("insert into preferences value ();", conn);
            cmd.ExecuteNonQuery();

            int preferencesId = 0;
            cmd = new MySqlCommand("select MAX(id) from preferences;", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                preferencesId = reader.GetInt32(0);
            }

            cmd = new MySqlCommand("INSERT INTO user (name, age, gender, preferences, role, login, password)" +
                                "VALUES(@name, @age, @gender, @preferences, 2, @login, @password);", conn);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@age", user.Age);
            cmd.Parameters.AddWithValue("@gender", user.Gender);
            cmd.Parameters.AddWithValue("@preferences", preferencesId);
            cmd.Parameters.AddWithValue("@login", user.Login);
            cmd.Parameters.AddWithValue("@password", user.Password);
            int affected = cmd.ExecuteNonQuery();

            if (affected == 0) return NotFound();
            conn.Close();
            conn.Dispose();
            return new JsonResult("Success!") { StatusCode = StatusCodes.Status201Created };
        }

        // Update status
        [HttpPut]
        public ActionResult Put(UserStatus userStatus)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"update user set status = @status where id = @id;", conn);
            cmd.Parameters.AddWithValue("@status", userStatus.Status);
            cmd.Parameters.AddWithValue("@id", userStatus.UserId);
            int affected = cmd.ExecuteNonQuery();
            if (affected == 0) return NotFound();
            conn.Close();
            conn.Dispose();
            return Ok();
        }
    }
}
