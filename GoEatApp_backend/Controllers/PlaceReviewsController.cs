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
    public class PlaceReviewsController : ControllerBase
    {
        MySqlConnection conn = DBUtils.GetDBConnection();

        // Get reviews by place id
        [HttpGet("{placeId}")]
        public async Task<ActionResult> GetReviewsByPlaceId(int placeId)
        {
            List<PlaceReview> reviews = new List<PlaceReview>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("call getReviewsByPlaceId(@placeId);", conn);
            cmd.Parameters.AddWithValue("@placeId", placeId);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    PlaceReview review;
                    while (reader.Read())
                    {
                        review = new PlaceReview();
                        review.Id = reader.GetInt32(0);
                        review.UserId = reader.GetInt32(1);
                        review.UserName = DBUtils.SafeGetString(reader, 2);
                        review.PlaceId = reader.GetInt32(3);
                        review.Score = reader.GetInt32(4);
                        review.Review = DBUtils.SafeGetString(reader, 5);
                        reviews.Add(review);
                    }
                }
                else return await Task.FromResult(NotFound());
            }
            conn.Close();
            conn.Dispose();
            return await Task.FromResult(new JsonResult(reviews));
        }

        // Create review
        [HttpPost]
        public ActionResult Post(PlaceReviewCreate review)
        {
            if (review == null) return BadRequest();
            conn.Open();

            MySqlCommand cmd;
            cmd = new MySqlCommand("insert into place_reviews (user, place, score, review) " +
                                    "values (@user, @place, @score, @review);", conn);
            cmd.Parameters.AddWithValue("@user", review.UserId);
            cmd.Parameters.AddWithValue("@place", review.PlaceId);
            cmd.Parameters.AddWithValue("@score", review.Score);
            cmd.Parameters.AddWithValue("@review", review.Review);
            int affected = cmd.ExecuteNonQuery();
            if (affected == 0) return BadRequest();

            conn.Close();
            conn.Dispose();
            return Ok();
        }
    }
}
