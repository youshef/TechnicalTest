using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using TechnicalTest.Data;
using TechnicalTest.Data.Models;
using TechnicalTest.Data.Services;
using Xunit;

namespace TechnicalTest.Tests
{
    public class FavoritesServiceTests
    {
        FavoritesService favoritesService = null;
        AdsDbContext ctx = null;
        public FavoritesServiceTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<AdsDbContext>()
                 .UseSqlite(connection)
                .Options;
            var logger = new Mock<ILogger<AdsDbContext>>();
            var servicelogger = new Mock<ILogger<FavoritesService>>();
            ctx = new AdsDbContext(options, logger.Object);
            ctx.SeetTestData();
            favoritesService = new FavoritesService(ctx, servicelogger.Object);
        }
        [Fact]
        public void DbContextShouldContainThreeUsers()
        {
            var users = ctx.Users.ToList();
            Assert.NotEmpty(users);
            Assert.Equal(3, users.Count);
        }
        [Fact]
        void Should_Favorite_An_Ad_Successfully_With_Correct_Ad_Id_And_User_Id_And_Manual_Mode()
        {
            var userId = ctx.Users.First(x => x.Username == "User1").Id;
            var adId = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var result = favoritesService.FavoriteAnAd(adId, userId, SaveTypes.Manual);
            Assert.True(result);
            Assert.Equal(1, ctx.Favorites.Where(x => x.AdId == adId && x.UserId == userId).Count());
        }
        [Fact]
        void Should_Favorite_An_Ad_Successfully_With_Correct_Ad_Id_And_User_Id_And_Automatic_Mode()
        {
            var userId = ctx.Users.First(x => x.Username == "User1").Id;
            var adId = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var result = favoritesService.FavoriteAnAd(adId, userId, SaveTypes.Manual);
            Assert.True(result);
            Assert.Equal(1, ctx.Favorites.Where(x => x.AdId == adId && x.UserId == userId).Count());
        }
        [Fact]
        void Should_Not_Favorite_An_Ad_Because_its_already_favorited()
        {
            var userId = ctx.Users.First(x => x.Username == "User1").Id;
            var adId = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var result = favoritesService.FavoriteAnAd(adId, userId, SaveTypes.Automatic);
            var result2 = favoritesService.FavoriteAnAd(adId, userId, SaveTypes.Manual);
            Assert.True(result);
            Assert.False(result2);
            Assert.Equal(1, ctx.Favorites.Where(x => x.AdId == adId && x.UserId == userId).Count());
        }
        [Fact]
        void Should_Not_Favorite_An_Ad_Because_its_type_is_invalid()
        {
            var userId = ctx.Users.First(x => x.Username == "User1").Id;
            var adId = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var result = favoritesService.FavoriteAnAd(adId, userId, "Unknown");
            Assert.False(result);
            Assert.Equal(0, ctx.Favorites.Where(x => x.AdId == adId && x.UserId == userId).Count());

        }
        [Fact]
        void Should_Not_Favorite_An_Ad_Because_inexisting_user_id()
        {
            var userId = Guid.NewGuid();
            var adId = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var result = favoritesService.FavoriteAnAd(adId, userId, "Unknown");
            Assert.False(result);
            Assert.Equal(0, ctx.Favorites.Where(x => x.AdId == adId && x.UserId == userId).Count());

        }
        [Fact]
        void Should_Not_Favorite_An_Ad_Because_inexisting_ad_id()
        {
            var userId = ctx.Users.First(x => x.Username == "User1").Id;
            var adId = Guid.NewGuid();
            var result = favoritesService.FavoriteAnAd(adId, userId, "Unknown");
            Assert.False(result);
            Assert.Equal(0, ctx.Favorites.Where(x => x.AdId == adId && x.UserId == userId).Count());

        }
        [Fact]
        void Should_Not_Favorite_An_Ad_Because_both_ad_and_user_dont_exist()
        {
            var userId = Guid.NewGuid();
            var adId = Guid.NewGuid();
            var result = favoritesService.FavoriteAnAd(adId, userId, "Unknown");
            Assert.False(result);
            Assert.Equal(0, ctx.Favorites.Where(x => x.AdId == adId && x.UserId == userId).Count());

        }

        [Fact]
        void Should_retrieve_all_favorited_ads_for_user()
        {
            var userId1 = ctx.Users.First(x => x.Username == "User1").Id;
            var userId2 = ctx.Users.First(x => x.Username == "User2").Id;
            var adId1 = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var adId2 = ctx.Ads.First(x => x.Title == "Ad2").Id;
            var adId3 = ctx.Ads.First(x => x.Title == "Ad3").Id;
            var adId4 = ctx.Ads.First(x => x.Title == "Ad4").Id;
            //two automatic saves for user1
            var result = favoritesService.FavoriteAnAd(adId1, userId1, SaveTypes.Automatic);
            Assert.True(result);
            var result2 = favoritesService.FavoriteAnAd(adId2, userId1, SaveTypes.Automatic);
            Assert.True(result2);
            //two manual saves for user1
            var result3 = favoritesService.FavoriteAnAd(adId3, userId1, SaveTypes.Manual);
            Assert.True(result3);
            var result4 = favoritesService.FavoriteAnAd(adId4, userId1, SaveTypes.Manual);
            Assert.True(result4);
            //one automatic save for user2
            var result5 = favoritesService.FavoriteAnAd(adId1, userId2, SaveTypes.Automatic);
            Assert.True(result5);
            //one manual save for user2
            var result6 = favoritesService.FavoriteAnAd(adId2, userId2, SaveTypes.Automatic);
            Assert.True(result6);

            var user1Ads = favoritesService.RetrieveFavoritedAds(userId1);
            Assert.Equal(4, user1Ads.Count());
            
            var user2Ads = favoritesService.RetrieveFavoritedAds(userId2);
            Assert.Equal(2, user2Ads.Count());

        }
        [Fact]
        void Should_retrieve_all_manually_favorited_ads_for_user()
        {
            var userId1 = ctx.Users.First(x => x.Username == "User1").Id;
            var userId2 = ctx.Users.First(x => x.Username == "User2").Id;
            var adId1 = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var adId2 = ctx.Ads.First(x => x.Title == "Ad2").Id;
            var adId3 = ctx.Ads.First(x => x.Title == "Ad3").Id;
            var adId4 = ctx.Ads.First(x => x.Title == "Ad4").Id;
            //two automatic saves for user1
            var result = favoritesService.FavoriteAnAd(adId1, userId1, SaveTypes.Automatic);
            Assert.True(result);
            var result2 = favoritesService.FavoriteAnAd(adId2, userId1, SaveTypes.Automatic);
            Assert.True(result2);
            //two manual saves for user1
            var result3 = favoritesService.FavoriteAnAd(adId3, userId1, SaveTypes.Manual);
            Assert.True(result3);
            var result4 = favoritesService.FavoriteAnAd(adId4, userId1, SaveTypes.Manual);
            Assert.True(result4);
            //one automatic save for user2
            var result5 = favoritesService.FavoriteAnAd(adId1, userId2, SaveTypes.Automatic);
            Assert.True(result5);
            //one manual save for user2
            var result6 = favoritesService.FavoriteAnAd(adId2, userId2, SaveTypes.Manual);
            Assert.True(result6);

            var user1Ads = favoritesService.RetrieveFavoritedAds(userId1,SaveTypes.Manual);
            Assert.Equal(2, user1Ads.Count());

            var user2Ads = favoritesService.RetrieveFavoritedAds(userId2, SaveTypes.Manual);
            Assert.Single(user2Ads);

        }
        [Fact]
        void Should_retrieve_all_automatically_favorited_ads_for_user()
        {
            var userId1 = ctx.Users.First(x => x.Username == "User1").Id;
            var userId2 = ctx.Users.First(x => x.Username == "User2").Id;
            var adId1 = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var adId2 = ctx.Ads.First(x => x.Title == "Ad2").Id;
            var adId3 = ctx.Ads.First(x => x.Title == "Ad3").Id;
            var adId4 = ctx.Ads.First(x => x.Title == "Ad4").Id;
            //two automatic saves for user1
            var result = favoritesService.FavoriteAnAd(adId1, userId1, SaveTypes.Automatic);
            Assert.True(result);
            var result2 = favoritesService.FavoriteAnAd(adId2, userId1, SaveTypes.Automatic);
            Assert.True(result2);
            //two manual saves for user1
            var result3 = favoritesService.FavoriteAnAd(adId3, userId1, SaveTypes.Manual);
            Assert.True(result3);
            var result4 = favoritesService.FavoriteAnAd(adId4, userId1, SaveTypes.Manual);
            Assert.True(result4);
            //one automatic save for user2
            var result5 = favoritesService.FavoriteAnAd(adId1, userId2, SaveTypes.Automatic);
            Assert.True(result5);
            //one manual save for user2
            var result6 = favoritesService.FavoriteAnAd(adId2, userId2, SaveTypes.Manual);
            Assert.True(result6);

            var user1Ads = favoritesService.RetrieveFavoritedAds(userId1, SaveTypes.Automatic);
            Assert.Equal(2, user1Ads.Count());

            var user2Ads = favoritesService.RetrieveFavoritedAds(userId2, SaveTypes.Automatic);
            Assert.Single(user2Ads);

        }
        [Fact]
        void Should_return_empty_list_of_favorited_ads_when_type_is_invalid()
        {
            var userId1 = ctx.Users.First(x => x.Username == "User1").Id;
            var userId2 = ctx.Users.First(x => x.Username == "User2").Id;
            var adId1 = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var adId2 = ctx.Ads.First(x => x.Title == "Ad2").Id;
            var adId3 = ctx.Ads.First(x => x.Title == "Ad3").Id;
            var adId4 = ctx.Ads.First(x => x.Title == "Ad4").Id;
            //two automatic saves for user1
            var result = favoritesService.FavoriteAnAd(adId1, userId1, SaveTypes.Automatic);
            Assert.True(result);
            var result2 = favoritesService.FavoriteAnAd(adId2, userId1, SaveTypes.Automatic);
            Assert.True(result2);
            //two manual saves for user1
            var result3 = favoritesService.FavoriteAnAd(adId3, userId1, SaveTypes.Manual);
            Assert.True(result3);
            var result4 = favoritesService.FavoriteAnAd(adId4, userId1, SaveTypes.Manual);
            Assert.True(result4);
            //one automatic save for user2
            var result5 = favoritesService.FavoriteAnAd(adId1, userId2, SaveTypes.Automatic);
            Assert.True(result5);
            //one manual save for user2
            var result6 = favoritesService.FavoriteAnAd(adId2, userId2, SaveTypes.Automatic);
            Assert.True(result6);

            var user1Ads = favoritesService.RetrieveFavoritedAds(userId1, "Unknown");
            Assert.Empty(user1Ads);

            var user2Ads = favoritesService.RetrieveFavoritedAds(userId2, "unknown");
            Assert.Empty(user2Ads);

        }

        [Fact]
        void Should_return_empty_list_of_favorited_ads_when_user_id_is_invalid()
        {
            var userId1 = ctx.Users.First(x => x.Username == "User1").Id;
            var userId2 = ctx.Users.First(x => x.Username == "User2").Id;
            var adId1 = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var adId2 = ctx.Ads.First(x => x.Title == "Ad2").Id;
            var adId3 = ctx.Ads.First(x => x.Title == "Ad3").Id;
            var adId4 = ctx.Ads.First(x => x.Title == "Ad4").Id;
            //two automatic saves for user1
            var result = favoritesService.FavoriteAnAd(adId1, userId1, SaveTypes.Automatic);
            Assert.True(result);
            var result2 = favoritesService.FavoriteAnAd(adId2, userId1, SaveTypes.Automatic);
            Assert.True(result2);
            //two manual saves for user1
            var result3 = favoritesService.FavoriteAnAd(adId3, userId1, SaveTypes.Manual);
            Assert.True(result3);
            var result4 = favoritesService.FavoriteAnAd(adId4, userId1, SaveTypes.Manual);
            Assert.True(result4);
            //one automatic save for user2
            var result5 = favoritesService.FavoriteAnAd(adId1, userId2, SaveTypes.Automatic);
            Assert.True(result5);
            //one manual save for user2
            var result6 = favoritesService.FavoriteAnAd(adId2, userId2, SaveTypes.Automatic);
            Assert.True(result6);

            var user1Ads = favoritesService.RetrieveFavoritedAds(Guid.NewGuid());
            Assert.Empty(user1Ads);

        }




        [Fact]
        void Should_remove_ad_from_favorites()
        {
            var userId1 = ctx.Users.First(x => x.Username == "User1").Id;
            var adId1 = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var adId2 = ctx.Ads.First(x => x.Title == "Ad2").Id;
            var adId3 = ctx.Ads.First(x => x.Title == "Ad3").Id;
            var adId4 = ctx.Ads.First(x => x.Title == "Ad4").Id;
            //two automatic saves for user1
            var result = favoritesService.FavoriteAnAd(adId1, userId1, SaveTypes.Automatic);
            Assert.True(result);
            var result2 = favoritesService.FavoriteAnAd(adId2, userId1, SaveTypes.Automatic);
            Assert.True(result2);
            //two manual saves for user1
            var result3 = favoritesService.FavoriteAnAd(adId3, userId1, SaveTypes.Manual);
            Assert.True(result3);
            var result4 = favoritesService.FavoriteAnAd(adId4, userId1, SaveTypes.Manual);
            Assert.True(result4);
            

            var user1Ads = favoritesService.RetrieveFavoritedAds(userId1);
            
            Assert.Equal(4, user1Ads.Count());

            var removeResult = favoritesService.RemoveAdFromFavorites(adId1, userId1);
            Assert.True(removeResult);

            var user1AdsAfterDelete = favoritesService.RetrieveFavoritedAds(userId1);
            Assert.Equal(3, user1AdsAfterDelete.Count());





        }
        [Fact]
        void Should_not_remove_ad_from_favorites_because_user_id_is_invalid()
        {
            var userId1 = ctx.Users.First(x => x.Username == "User1").Id;
            var adId1 = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var adId2 = ctx.Ads.First(x => x.Title == "Ad2").Id;
            var adId3 = ctx.Ads.First(x => x.Title == "Ad3").Id;
            var adId4 = ctx.Ads.First(x => x.Title == "Ad4").Id;
            //two automatic saves for user1
            var result = favoritesService.FavoriteAnAd(adId1, userId1, SaveTypes.Automatic);
            Assert.True(result);
            var result2 = favoritesService.FavoriteAnAd(adId2, userId1, SaveTypes.Automatic);
            Assert.True(result2);
            //two manual saves for user1
            var result3 = favoritesService.FavoriteAnAd(adId3, userId1, SaveTypes.Manual);
            Assert.True(result3);
            var result4 = favoritesService.FavoriteAnAd(adId4, userId1, SaveTypes.Manual);
            Assert.True(result4);


            var user1Ads = favoritesService.RetrieveFavoritedAds(userId1);
            
            Assert.Equal(4, user1Ads.Count());
            var removeResult = favoritesService.RemoveAdFromFavorites(adId1, Guid.NewGuid());
            Assert.False(removeResult);

            var user1AdsAfterDelete = favoritesService.RetrieveFavoritedAds(userId1);
            Assert.Equal(4, user1AdsAfterDelete.Count());





        }
        [Fact]
        void Should_not_remove_ad_from_favorites_because_ad_id_is_invalid()
        {
            var userId1 = ctx.Users.First(x => x.Username == "User1").Id;
            var adId1 = ctx.Ads.First(x => x.Title == "Ad1").Id;
            var adId2 = ctx.Ads.First(x => x.Title == "Ad2").Id;
            var adId3 = ctx.Ads.First(x => x.Title == "Ad3").Id;
            var adId4 = ctx.Ads.First(x => x.Title == "Ad4").Id;
            //two automatic saves for user1
            var result = favoritesService.FavoriteAnAd(adId1, userId1, SaveTypes.Automatic);
            Assert.True(result);
            var result2 = favoritesService.FavoriteAnAd(adId2, userId1, SaveTypes.Automatic);
            Assert.True(result2);
            //two manual saves for user1
            var result3 = favoritesService.FavoriteAnAd(adId3, userId1, SaveTypes.Manual);
            Assert.True(result3);
            var result4 = favoritesService.FavoriteAnAd(adId4, userId1, SaveTypes.Manual);
            Assert.True(result4);


            var user1Ads = favoritesService.RetrieveFavoritedAds(userId1);

            Assert.Equal(4, user1Ads.Count());
            var removeResult = favoritesService.RemoveAdFromFavorites(Guid.NewGuid(), userId1);
            Assert.False(removeResult);

            var user1AdsAfterDelete = favoritesService.RetrieveFavoritedAds(userId1);
            Assert.Equal(4, user1AdsAfterDelete.Count());





        }
    }
}
