using System.Collections.Generic;
using System.Threading.Tasks;
using GoEatapp_backend;
using GoEatapp_backend.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GoEatApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            Place place = new Place();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("call getPlaceByAddressId(" + id + ");", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    PutPlace(reader, ref place);

                    //System.Diagnostics.Debug.WriteLine(place.Name);
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(place));
        }

        [HttpGet("{requestArea}/{userLocation}/{cuisine_nationality}/{interior}")]
        public async Task<ActionResult> Get(string requestArea, string userLocation, string cuisine_nationality, string interior)
        {
            List<Place> places = new List<Place>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                "call getPlacesListByLocationAndPreferences(" + "\'" + requestArea + "\'," +
                                                                "\'" +  userLocation + "\'," +
                                                                "\'" +  cuisine_nationality + "\'," +
                                                                "\'" +  interior +
                                                                "\');", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Place place = new Place();
                    while (reader.Read())
                    {
                        PutPlace(reader, ref place);
                        places.Add(place);
                    }
                    
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(places));
        }

        private void PutPlace(MySqlDataReader reader, ref Place place)
        {
            place.Id = reader.GetInt32(0);
            place.Name = DBUtils.SafeGetString(reader, 1);
            place.Photo = DBUtils.SafeGetString(reader, 2);
            place.Country = DBUtils.SafeGetString(reader, 3);
            place.Region = DBUtils.SafeGetString(reader, 4);
            place.Town = DBUtils.SafeGetString(reader, 5);
            place.MailIndex = DBUtils.SafeGetString(reader, 6);
            place.Street = DBUtils.SafeGetString(reader, 7);
            place.House = DBUtils.SafeGetString(reader, 8);
            place.Apartment = DBUtils.SafeGetString(reader, 9);
            place.CuisineNationality = DBUtils.SafeGetString(reader, 10);
            place.Interior = DBUtils.SafeGetString(reader, 11);
            place.Tagline = DBUtils.SafeGetString(reader, 12);
            place.Other = DBUtils.SafeGetString(reader, 13);
        }
    }
}
