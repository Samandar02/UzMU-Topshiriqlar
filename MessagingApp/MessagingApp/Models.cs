using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp.Models
{
    public class Users
    {
        public int id { get; set; }
        public string UserName { get; set; }
    }
    public class Messages
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
    public class Credentials
    {
        public int Id { get; set; }
        public string userName { get; set; }
    }

}
