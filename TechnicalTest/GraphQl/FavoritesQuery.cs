using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalTest.Data.Models;
using TechnicalTest.Data.Services;

namespace TechnicalTest.Api.GraphQl
{
    public class FavoritesQuery : ObjectGraphType
    {
        public FavoritesQuery()
        {
            
        }
        public FavoritesQuery(IFavoritesService favoritesService)
        {
            Field<ListGraphType<AdType>>(
              "favs",
              arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "userId", Description = "User Id" }
                    ),
              resolve: context =>
              {
                  var userId = context.GetArgument<int>("userId");
                  return favoritesService.RetrieveFavoritedAds(userId).ToList();
              }
            );
            Field<ListGraphType<AdType>>(
              "manualFavs",
              arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "userId", Description = "User Id" }
                    ),
              resolve: context =>
              {
                  var userId = context.GetArgument<int>("userId");

                  return favoritesService.RetrieveFavoritedAds(userId,SaveTypes.Manual).ToList();
              }
            );
            Field<ListGraphType<AdType>>(
              "automaticFavs",
              arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "userId", Description = "User Id" }
                    ),
              resolve: context =>
              {
                  var userId = context.GetArgument<int>("userId");

                  return favoritesService.RetrieveFavoritedAds(userId, SaveTypes.Automatic).ToList();
              }
            );


        }
    }
}
