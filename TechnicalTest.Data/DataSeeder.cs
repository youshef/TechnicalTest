using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Data
{
    public static class DataSeeder
    {
        public static void SeetTestData(this AdsDbContext db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));
            //if (db.Ads.Any())
            //    return;

            db.Favorites.RemoveRange(db.Favorites);
            db.Ads.RemoveRange(db.Ads);
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
            db._logger.LogInformation("Seeding database");
            var user1 = new User("User1") { Id = 1 };
            var user2 = new User("User2") { Id =2 };
            var user3 = new User("User3") { Id = 3 };

            db.Users.Add(user1);
            db.Users.Add(user2);
            db.Users.Add(user3);
            db.SaveChanges();
            var ad1 = new Ad("Ad1") { Id =  1 };
            var ad2 = new Ad("Ad2"){ Id =  2 };
            var ad3 = new Ad("Ad3"){ Id =  3 };
            var ad4 = new Ad("Ad4"){ Id =  4 };
            var ad5 = new Ad("Ad5"){ Id =  5 };
            var ad6 = new Ad("Ad6"){ Id =  6 };
            var ad7 = new Ad("Ad7"){ Id =  7 };

            db.Ads.Add(ad1);
            db.Ads.Add(ad2);
            db.Ads.Add(ad3);
            db.Ads.Add(ad4);
            db.Ads.Add(ad5);
            db.Ads.Add(ad6);
            db.Ads.Add(ad7);
            db.SaveChanges();

            var f1 = new Favorite(user3.Id, ad1.Id, SaveTypes.Automatic);
            var f2 = new Favorite(user3.Id, ad2.Id, SaveTypes.Manual);
            var f3 = new Favorite(user3.Id, ad3.Id, SaveTypes.Automatic);
            var f4 = new Favorite(user3.Id, ad4.Id, SaveTypes.Manual);

            db.Favorites.Add(f1);
            db.Favorites.Add(f2);
            db.Favorites.Add(f3);
            db.Favorites.Add(f4);
            db.SaveChanges();

        }
    }
}
