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
            if (db.Ads.Any())
                return;

            db.Favorites.RemoveRange(db.Favorites);
            db.Ads.RemoveRange(db.Ads);
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
            db._logger.LogInformation("Seeding database");
            var user1 = new User("User1") { Id = new Guid("e8c28b03-d12f-4bee-b612-6c60781d4486") };
            var user2 = new User("User2") { Id = new Guid("25ea2736-969c-4d87-b49e-dea2742d7455") };
            var user3 = new User("User3") { Id = new Guid("792da03a-8a19-484c-921e-dad4c116868c") };

            db.Users.Add(user1);
            db.Users.Add(user2);
            db.Users.Add(user3);
            db.SaveChanges();
            var ad1 = new Ad("Ad1") { Id = new Guid("473ce341-290c-478c-afcc-f0824cfc70b4") };
            var ad2 = new Ad("Ad2"){ Id = new Guid("6afb342d-8592-47a9-85ec-5f56d4e743f9") };
            var ad3 = new Ad("Ad3"){ Id = new Guid("9ade84d5-88d4-4499-a2c0-b9e0511ac759") };
            var ad4 = new Ad("Ad4"){ Id = new Guid("eec8bde6-8668-4b06-9191-53ee52cd9bce") };
            var ad5 = new Ad("Ad5"){ Id = new Guid("ee1d6cfa-da5e-4c0c-805e-59f746898992") };
            var ad6 = new Ad("Ad6"){ Id = new Guid("b8093688-6974-4012-9cb9-abe61813893b") };
            var ad7 = new Ad("Ad7") { Id = new Guid("75f7e7d7-fcc1-4ed2-aea6-1736a278b37a") };

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
