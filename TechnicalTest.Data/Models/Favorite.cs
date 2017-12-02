using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalTest.Data.Models
{
    public class Favorite : IEntity
    {
        public Favorite(int userId, int adId, string saveType)
        {
            UserId = userId;
            AdId = adId;
            SaveType = saveType ?? throw new ArgumentNullException(nameof(saveType));
        }

        public Favorite()
        {

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int AdId { get; set; }
        public Ad Ad { get; set; }
        //To differentiate between Automatic and manual  , Values {Automatic,Manual}
        public string SaveType { get; set; }
    }
}
