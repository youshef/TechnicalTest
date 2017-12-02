using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Data.Services;
using Microsoft.Extensions.Logging;
using TechnicalTest.Data.Models;
using TechnicalTest.Data;

namespace TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    public class FavoritesController : Controller
    {
        private readonly IFavoritesService _favoritesService;
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(IFavoritesService favoritesService ,ILogger<FavoritesController> logger,AdsDbContext dbContext)
        {
            _favoritesService = favoritesService;
            _logger = logger;
            dbContext.SeetTestData();
            

        }
        //for All : GET http://localhost:51052/api/favorites/3/
        //for manually saved ads : GET http://localhost:51052/api/favorites/3/Manual
        //for automatically saved ads : GET http://localhost:51052/api/favorites/3/Automatic
        [HttpGet("{userId}")]
        public IEnumerable<Ad> Get(int userId,string type="")
        {
            if(string.IsNullOrWhiteSpace(type))
            return _favoritesService.RetrieveFavoritedAds(userId, "All");
            return _favoritesService.RetrieveFavoritedAds(userId, type);
        }
        //Example : POST http://localhost:51052/api/favorites/7/3/Manual
        [HttpPost("{adId}/{userId}/{type}")]
        public bool AdSave(int adId, int userId,string type)
        {
            return _favoritesService.FavoriteAnAd(adId, userId, type);
        }
        


        //Example : DELETE http://localhost:51052/api/favorites/7/3
        [HttpDelete("{adId}/{userId}")]
        public bool Delete(int adId,int userId)
        {
            return _favoritesService.RemoveAdFromFavorites(adId, userId);
        }
    }
}

