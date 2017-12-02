using GraphQL.Types;
using System;
using System.Linq;

namespace TechnicalTest.Api.GraphQl
{
    public class FavoritesSchema : Schema
    {
        public FavoritesSchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (FavoritesQuery)resolveType(typeof(FavoritesQuery));
        }
    }
}
