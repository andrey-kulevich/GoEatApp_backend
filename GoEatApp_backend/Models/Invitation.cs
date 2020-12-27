using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoEatapp_backend.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public string DateTime { get; set; }
        public Place Place { get; set; }
        public int WhoWillPay { get; set; }
        public string Message { get; set; }
        public int SenderId { get; set; }
        public string Sender { get; set; }
        public int RecipientId { get; set; }
        public string Recipient { get; set; }
        public bool Accepted { get; set; }
    }

    public class InvitationCreate
    {
        public string DateTime { get; set; }
        public int Address { get; set; }
        public int WhoWillPay { get; set; }
        public string Message { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
    }
}
