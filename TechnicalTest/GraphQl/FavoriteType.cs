using GraphQL.Types;
using System;
using System.Linq;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Api.GraphQl
{
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
}
