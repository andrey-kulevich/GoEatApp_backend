using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoEatapp_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public Preferences Preferences { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
