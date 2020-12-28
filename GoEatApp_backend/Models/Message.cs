using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoEatapp_backend.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string DateTime { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        //public string SenderName { get; set; }
        public int RecipientId { get; set; }
        //public string RecipientName { get; set; }
    }

    public class MessageCreate
    {
        public string DateTime { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
    }

    public class Dialog
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public string Avatar { get; set; }
    }
}
