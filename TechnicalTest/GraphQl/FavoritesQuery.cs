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
    class FavoriteInterface : InterfaceGraphType<Favorite>
    {
        public FavoriteInterface()
        {
            Name = "Favorite";
            Field(d => d.UserId).Description("The id of the user.");
            Field(d => d.AdId).Description("the id of the Ad");
            Field(d => d.SaveType).Description("Type of the favorite");
        }
    }
    class FavoriteType : ObjectGraphType<Favorite>
    {
        public FavoriteType()
        {
            Name = "Favorite";
            Field(d => d.UserId).Description("The id of the user.");
            Field(d => d.AdId).Description("the id of the Ad");
            Field(d => d.SaveType).Description("Type of the favorite");
        }
    }
    class AdInterface : InterfaceGraphType<Ad>
    {
        public AdInterface()
        {
            Name = "Ad";
            Field(d => d.Id).Description("The id of the ad.");
            Field(d => d.Title).Description("the title of the Ad");
        }
    }
    public class AdType : ObjectGraphType<Ad>
    {
        public AdType()
        {
            Name = "Ad";
            Field(x => x.Id, type: typeof(IntGraphType)).Description("Ad Id");
            Field(x => x.Title).Description("Ad Title");
       }
    }
}
