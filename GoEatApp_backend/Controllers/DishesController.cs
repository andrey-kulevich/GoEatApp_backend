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
    public class DishesController : ControllerBase
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        // Get dish by id
        [HttpGet("{dishId}")]
        public async Task<ActionResult> GetDishById(int dishId)
        {
            conn.Open();
            Dish dish = GetDish(dishId, conn);
            conn.Close();
            conn.Dispose();
            if (dish == null) return await Task.FromResult(NotFound());
            return await Task.FromResult(new JsonResult(dish));
        }

        // Get all dishes
        [HttpGet]
        public async Task<ActionResult> GetAllDishes()
        {
            List<Dish> dishes = new List<Dish>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getAllDishes();", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Dish dish;
                    while (reader.Read())
                    {
                        dish = new Dish();
                        PutDish(reader, ref dish);
                        dishes.Add(dish);
                    }
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(dishes));
        }

        // Get dishes by place id
        [HttpGet("place/{placeId}")]
        public async Task<ActionResult> GetDishesByPlaceId(int placeId)
        {
            List<Dish> dishes = new List<Dish>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("call getDishesByPlaceId(@placeId);", conn);
            cmd.Parameters.AddWithValue("@placeId", placeId);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Dish dish;
                    while (reader.Read())
                    {
                        dish = new Dish();
                        PutDish(reader, ref dish);
                        dishes.Add(dish);
                    }
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(dishes));
        }

        public static Dish GetDish(int dishId, MySqlConnection conn)
        {
            Dish dish = new Dish();
            MySqlCommand cmd = new MySqlCommand("call getDishById(@dishId);", conn);
            cmd.Parameters.AddWithValue("@dishId", dishId);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    PutDish(reader, ref dish);
                }
                else return null;
            }
            return dish;
        }

        private static void PutDish(MySqlDataReader reader, ref Dish dish)
        {
            dish.Id = reader.GetInt32(0);
            dish.Name = DBUtils.SafeGetString(reader, 1);
            dish.Photo = DBUtils.SafeGetString(reader, 2);
            dish.DishType = DBUtils.SafeGetString(reader, 3);
            dish.Recipe = DBUtils.SafeGetString(reader, 4);
            dish.CuisineNationality = DBUtils.SafeGetString(reader, 5);
        }
    }
}
