
namespace GoEatapp_backend.Models
{
    public class PlaceReview
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PlaceId { get; set; }
        public int Score { get; set; }
        public string Review { get; set; }
    }

    public class PlaceReviewCreate
    {
        public int UserId { get; set; }
        public int PlaceId { get; set; }
        public int Score { get; set; }
        public string Review { get; set; }
    }
}
