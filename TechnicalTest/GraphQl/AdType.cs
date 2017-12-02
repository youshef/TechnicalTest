using GraphQL.Types;
using System;
using System.Linq;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Api.GraphQl
{
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
