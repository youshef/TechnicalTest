using System;
using System.Collections.Generic;
using System.Linq;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Data.Services
{


    public interface IFavoritesService
    {
        bool FavoriteAnAd(Guid adId, Guid userId, string saveType);
        IEnumerable<Ad> RetrieveFavoritedAds(Guid userId, string type = "All");

        bool RemoveAdFromFavorites(Guid adId, Guid userId);
    }
}
