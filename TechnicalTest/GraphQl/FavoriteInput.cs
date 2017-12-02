using System;
using System.Linq;

namespace TechnicalTest.Api.GraphQl
{
    public class FavoriteInput
    {
        public int AdId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }

    }
}
