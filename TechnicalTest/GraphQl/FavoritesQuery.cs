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
        public FavoritesQuery(IFavoritesService favoritesService)
        {
            Name = "FF";
            
            Field<ListGraphType<AdType>>("favorites",
                            arguments: new QueryArguments(
                              new QueryArgument<StringGraphType>() { Name = "userId" }, 
                              new QueryArgument<StringGraphType>() { Name = "type" }),
                              resolve: context =>
                              {
                                  var userIdString = context.GetArgument<string>("userId");
                                  var type = context.GetArgument<string>("type");
                                  var userIdGuid = Guid.Parse(userIdString);
                                  return favoritesService.RetrieveFavoritedAds(userIdGuid,type);
                              });

            //Field<ListGraphType<AdType>>("favs",
            //                               resolve: context =>
            //                               {
            //                                   return favoritesService.AllBooks();
            //                               });
        }
    }
    public class AdType : ObjectGraphType<Ad>
    {
        public AdType()
        {
            Field(x => x.Id, type: typeof(StringGraphType)).Description("Ad Id");
            Field(x => x.Title).Description("Ad Title");
       }
    }
}
