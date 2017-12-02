using GraphQL.Types;
using System;
using System.Linq;

namespace TechnicalTest.Api.GraphQl
{
    public class FavoriteInputType : InputObjectGraphType<FavoriteInput>
    {
        public FavoriteInputType()
        {
            Name = "FavoriteInput";
            Field(x => x.AdId).Name("adId");
            Field(x => x.UserId).Name("userId");
            Field(x => x.Type).Name("type");

        }
    }
}
