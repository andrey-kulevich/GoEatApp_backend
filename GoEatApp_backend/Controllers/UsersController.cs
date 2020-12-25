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

namespace GoEatApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        //[HttpGet]
        //public async Task<JsonResult> Get()
        //{
        //    List<User> users = new List<User>(); 

        //    MySqlCommand cmd = new MySqlCommand("select * from user;", conn);
        //    using (DbDataReader reader = cmd.ExecuteReader())
        //    {
        //        if (reader.HasRows)
        //        {
        //            User user = new User();
        //            while (reader.Read())
        //            {
        //                user.Id = reader.GetInt32()
        //            }
        //        }
        //    }

        //    return await Task.FromResult(new JsonResult(users));   
        //}
    }
}
