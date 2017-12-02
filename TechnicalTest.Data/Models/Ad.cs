using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechnicalTest.Data.Models
{
    public class Ad : IEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Title { get; set; }
        public Ad()
        {
            
        }

        public Ad( string title)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }
    }
}
