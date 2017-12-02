using System;

namespace TechnicalTest.Data.Models
{
    public class User : IEntity
    {

        public Guid Id { get; set; } 
        public string Username { get; set; }
        public User()
        {

        }

        public User(string username)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}
