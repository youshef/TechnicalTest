using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalTest.Data.Models
{
    public class User : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } 
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
