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
            //new AdsDbContext().SeetTestData();

        }
        [HttpGet("{userId}/{type}")]
        public IEnumerable<Ad> Get(Guid userId,string type)
        {
            if(string.IsNullOrWhiteSpace(type))
            return _favoritesService.RetrieveFavoritedAds(userId, "All");
            return _favoritesService.RetrieveFavoritedAds(userId, type);
        }

        [HttpPut,HttpPost]
        public bool AutomaticAdSave([FromBody]Guid adId, [FromBody]Guid userId)
        {
            return _favoritesService.FavoriteAnAd(adId, userId, SaveTypes.Automatic);
        }
        [HttpPut, HttpPost]
        public bool ManualAdSave([FromBody]Guid adId, [FromBody]Guid userId)
        {
            return _favoritesService.FavoriteAnAd(adId, userId, SaveTypes.Manual);
        }

        
        // DELETE api/adId/userId
        [HttpDelete("{adId}/{userId}")]
        public bool Delete(Guid adId,Guid userId)
        {
            return _favoritesService.RemoveAdFromFavorites(adId, userId);
        }
    }
}

