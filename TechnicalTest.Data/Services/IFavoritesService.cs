using System;
using System.Collections.Generic;
using System.Linq;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Data.Services
{


    public interface IFavoritesService
    {
        bool FavoriteAnAd(int adId, int userId, string saveType);
        IEnumerable<Ad> RetrieveFavoritedAds(int userId, string type = "All");

        bool RemoveAdFromFavorites(int adId, int userId);
    }
}
