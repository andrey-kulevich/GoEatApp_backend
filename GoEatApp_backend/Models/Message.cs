using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoEatapp_backend.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
    }
}
