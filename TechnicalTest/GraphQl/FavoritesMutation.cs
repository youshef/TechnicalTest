using GraphQL.Types;
using System;
using System.Linq;
using TechnicalTest.Data.Services;

namespace TechnicalTest.Api.GraphQl
{
    public class FavoritesMutation : ObjectGraphType<object>
    {

        public FavoritesMutation(IFavoritesService favoritesService)
        {
            Field<FavoriteInputType>(
            "createFavorite",
            arguments: new QueryArguments(
                new QueryArgument<FavoriteInputType> { Name = "fav" }

            ),
            resolve: context =>
            {
                var data = context.GetArgument<FavoriteInput>("fav");

                var result = favoritesService.FavoriteAnAd(data.AdId, data.UserId, data.Type);
                return result;
            });




            Field<FavoriteInputType>(
           "removeFavorite",
           arguments: new QueryArguments(
               new QueryArgument<IntGraphType> { Name = "userId" },
               new QueryArgument<IntGraphType> { Name = "adId" }

           ),
           resolve: context =>
           {
               var userId = context.GetArgument<int>("userId");
               var adId = context.GetArgument<int>("adId");

               var result = favoritesService.RemoveAdFromFavorites(adId,userId);
               return result;
           });
        }
    }
}
