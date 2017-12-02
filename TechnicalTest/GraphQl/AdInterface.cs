using GraphQL.Types;
using System;
using System.Linq;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Api.GraphQl
{
    class AdInterface : InterfaceGraphType<Ad>
    {
        public AdInterface()
        {
            Name = "Ad";
            Field(d => d.Id).Description("The id of the ad.");
            Field(d => d.Title).Description("the title of the Ad");
        }
    }
}
