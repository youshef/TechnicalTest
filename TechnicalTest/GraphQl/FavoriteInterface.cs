using GraphQL.Types;
using System;
using System.Linq;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Api.GraphQl
{
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
}
