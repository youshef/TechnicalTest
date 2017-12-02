using System;
using System.Collections.Generic;
using System.Text;

namespace TechnicalTest.Data.Models
{
    public class Ad : IEntity
    {

        public Guid Id { get; set; }
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
