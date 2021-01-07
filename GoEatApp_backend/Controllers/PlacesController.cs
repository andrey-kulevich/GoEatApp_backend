﻿using System.Collections.Generic;
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

        // Get place by address id
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            Place place = new Place();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getPlaceByAddressId({id});", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    PutPlace(reader, ref place);
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(place));
        }

        // Get places list by location and preferences
        [HttpGet("{requestArea}/{userLocation}/{cuisine_nationality}/{interior}")]
        public async Task<ActionResult> Get(string requestArea, string userLocation, string cuisine_nationality, string interior)
        {
            List<Place> places = new List<Place>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(
                $"call getPlacesListByLocationAndPreferences('{requestArea}', '{userLocation}', '{cuisine_nationality}', '{interior}');", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Place place;
                    while (reader.Read())
                    {
                        place = new Place();
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

        // Get favorite places
        [HttpGet("favorite/{userId}")]
        public async Task<ActionResult> GetFavotitePlaces(int userId)
        {
            List<Place> places = new List<Place>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"call getFavoritePlaces({userId});", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Place place;
                    while (reader.Read())
                    {
                        place = new Place();
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

        //Create place
        [HttpPost]
        public ActionResult Post(PlaceCreate place)
        {
            if (place == null) return BadRequest();
            conn.Open();

            MySqlCommand cmd;
            cmd = new MySqlCommand("insert into place " +
                            "(name, photo, cuisine_nationality, interior, tagline, other) " +
                            $"values ('{place.Name}', '{place.Photo}', {place.CuisineNationality}, {place.Interior}, " +
                            $"'{place.Tagline}', '{place.Other}');", conn);
            int affected = cmd.ExecuteNonQuery();

            if (affected == 0) return BadRequest();

            int placeId = 0;
            cmd = new MySqlCommand("select MAX(id) from place;", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                placeId = reader.GetInt32(0);
            }

            cmd = new MySqlCommand("insert into address " +
                            "(country, region, town, mail_index, street, house, apartment, place) " +
                            $"values ('{place.Country}', '{place.Region}', '{place.Town}', '{place.MailIndex}', " +
                            $"'{place.Street}', '{place.House}', '{place.Apartment}', {placeId});", conn);
            affected = cmd.ExecuteNonQuery();
            if (affected == 0) return BadRequest();

            conn.Close();
            conn.Dispose();
            return Ok();
        }

        //Create empty place
        [HttpPost("empty")]
        public ActionResult PostEmptyPlace(EmptyPlaceCreate place)
        {
            if (place == null) return BadRequest();
            conn.Open();

            MySqlCommand cmd;
            cmd = new MySqlCommand($"insert into place (name) values ('{place.Name}');", conn);
            int affected = cmd.ExecuteNonQuery();

            if (affected == 0) return BadRequest();

            int placeId = 0;
            cmd = new MySqlCommand("select MAX(id) from place;", conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                placeId = reader.GetInt32(0);
            }

            cmd = new MySqlCommand("insert into address " +
                            "(country, region, town, mail_index, street, house, apartment, place) " +
                            $"values ('{place.Country}', '{place.Region}', '{place.Town}', '{place.MailIndex}', " +
                            $"'{place.Street}', '{place.House}', '{place.Apartment}', {placeId});", conn);
            affected = cmd.ExecuteNonQuery();
            if (affected == 0) return BadRequest();

            conn.Close();
            conn.Dispose();
            return Ok();
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
