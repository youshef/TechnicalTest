using System;

namespace TechnicalTest.Data.Models
{
    public class Favorite : IEntity
    {
        public Favorite(Guid userId, Guid adId, string saveType)
        {
            UserId = userId;
            AdId = adId;
            SaveType = saveType ?? throw new ArgumentNullException(nameof(saveType));
        }

        public Favorite()
        {

        }
        public Guid Id { get; set; } 
        public Guid UserId { get; set; }
        public Guid AdId { get; set; }
        public Ad Ad { get; set; }
        //To differentiate between Automatic and manual  , Values {Automatic,Manual}
        public string SaveType { get; set; }
    }
}
