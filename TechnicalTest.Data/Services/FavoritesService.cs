using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Data.Services
{ 
public class FavoritesService : IFavoritesService
    {
        //The Service could implement verbose logging but i chose to just log the exceptions

        private readonly AdsDbContext _dbContext;
        private readonly ILogger<FavoritesService> _logger;
        
        public FavoritesService(AdsDbContext dbContext,ILogger<FavoritesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            
        }
        /// <summary>
        /// Saves an ad as a Favorite for a specific user,
        /// </summary>
        /// <param name="adId">Id of the Ad to save</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="saveType">Type of the favorite ("Automatic" or "Manual")</param>
        /// <returns>true if the Ad is saved successfully, false if an exception occured</returns>
        public bool FavoriteAnAd(Guid adId,Guid userId,string saveType)
        {
            try
            {
                //check the validity of the save type
                if (saveType != SaveTypes.Automatic && saveType != SaveTypes.Manual)
                    return false;
                //check if the both the user and the ad exists
                if (_dbContext.Ads.Find(adId) == null || _dbContext.Users.Find(userId) == null)
                    return false;
                //check if the ad is already saved by this user, to prevent double saving
                if (_dbContext.Favorites.Any(x => x.UserId == userId && x.AdId == adId))
                    return false;
                var favroties = new Favorite() { AdId = adId, UserId = userId, SaveType = saveType };
                _dbContext.Favorites.Add(favroties);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Saving the favorite");
            }
            return false;

        }
        /// <summary>
        /// Retrieves the favorited ads
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="type">Type of favorited ads : Manual,Automatic or All (defaulted to all)</param>
        /// <returns>the list of the saved Ads </returns>
        public IEnumerable<Ad> RetrieveFavoritedAds(Guid userId,string type="All")
        {

            try
            {
                //check if the the user exists, return empty list if not
                if (_dbContext.Users.Find(userId) == null)
                    return new List<Ad>();
                var lstFavorites = _dbContext.Favorites.Include(x=>x.Ad).Where(x => x.UserId == userId);
                if (type == "All")
                {
                    var r = lstFavorites.Select(x => x.Ad).ToList();
                    return r;
                }
                if (type == SaveTypes.Manual || type == SaveTypes.Automatic)
                {
                    return lstFavorites.Where(f => f.SaveType == type).Select(l => l.Ad).ToList();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving the favorited ads where type={type}");
            }
           

            //return empty collection when invalid type is specified
            return new List<Ad>();
        }
        //probably should verify if the favorited ad belongs to the user
        /// <summary>
        /// Deletes a favorited Ad provided the Id of the saved Ad and the user Id
        /// </summary>
        /// <param name="favoriteId">Id of the ad to remove from favorites</param>
        /// <returns>true if the favorite is removed successfully, false if an exception occured</returns>
        public bool RemoveAdFromFavorites(Guid adId,Guid userId)
        {

            try
            {
                //check if the ad is actually favorited by the user
               
                var favorite = _dbContext.Favorites.FirstOrDefault(x => x.UserId == userId && x.AdId == adId);
                if (favorite != null)
                {
                    _dbContext.Favorites.Remove(favorite);
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while removing the ad from the favorites");
            }
           
            return false;
        }




    }
}
