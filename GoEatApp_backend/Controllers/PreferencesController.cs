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
    public class PreferencesController : ControllerBase
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        // Get preferences by user id
        [HttpGet("{userId}")]
        public async Task<ActionResult> Get(int userId)
        {
            Preferences preferences = new Preferences();
            int bestDrinkId = 0;
            int bestFirstMealId = 0;
            int bestSecondMealId = 0;
            int bestDessertId = 0;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getPreferencesByUserId({userId});", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    preferences.Id = reader.GetInt32(0);
                    preferences.CiusineNationality = DBUtils.SafeGetString(reader, 1);
                    preferences.Interior = DBUtils.SafeGetString(reader, 2);
                    preferences.TipsPercentage = DBUtils.SafeGetIntId(reader, 3);
                    preferences.IsVegan = reader.GetInt32(4) == 1 ? true : false;
                    preferences.IsRawFood = reader.GetInt32(5) == 1 ? true : false;
                    bestDrinkId = DBUtils.SafeGetIntId(reader, 6);
                    bestFirstMealId = DBUtils.SafeGetIntId(reader, 7);
                    bestSecondMealId = DBUtils.SafeGetIntId(reader, 8);
                    bestDessertId = DBUtils.SafeGetIntId(reader, 9);
                    preferences.Other = DBUtils.SafeGetString(reader, 10);
                }
                else return await Task.FromResult(NotFound());
            }
            preferences.BestDrink = DishesController.GetDish(bestDrinkId, conn);
            preferences.BestFirstMeal = DishesController.GetDish(bestFirstMealId, conn);
            preferences.BestSecondMeal = DishesController.GetDish(bestSecondMealId, conn);
            preferences.BestDessert = DishesController.GetDish(bestDessertId, conn);

            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(preferences));
        }

        // Update preferences field by preferences id
        [HttpPut]
        public ActionResult Put([FromBody] PreferencesIntFieldUpdate updateObj)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"update preferences set " +
                $"{updateObj.FieldToUpdate} = {updateObj.IntValue} where id = {updateObj.PreferencesId};", conn);
            int affected = cmd.ExecuteNonQuery();
            if (affected == 0) return NotFound();
            conn.Close();
            conn.Dispose();
            return Ok();
        }

        // Update preferences "Other" field by preferences id
        [HttpPut("{other}")]
        public ActionResult PutOther([FromBody] PreferencesOtherUpate updateObj)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"update preferences set " +
                $"other = '{updateObj.OtherValue}' where id = {updateObj.PreferencesId};", conn);
            int affected = cmd.ExecuteNonQuery();
            if (affected == 0) return NotFound();
            conn.Close();
            conn.Dispose();
            return Ok();
        }
    }
}
