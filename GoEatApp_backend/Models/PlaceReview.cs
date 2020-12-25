using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoEatapp_backend.Models
{
    public class PlaceReview
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string UserName { get; set; }
        public int PlaceId { get; set; }
        public int Score { get; set; }
        public string Review { get; set; }
    }
}
