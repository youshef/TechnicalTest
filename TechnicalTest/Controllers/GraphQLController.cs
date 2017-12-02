using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Api.GraphQl;
using GraphQL;
using GraphQL.Types;

namespace TechnicalTest.Api.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(IDocumentExecuter documentExecuter, ISchema schema)
        {
            _documentExecuter = documentExecuter;
            _schema = schema;
        }
        /*{ 
         * example of query to post
 "query":
  "query{
     favs(userId:3){
       id 
       title
     }
   }"
}
*/
        [HttpPost]
        public async Task<ActionResult> Get([FromBody]GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var executionOptions = new ExecutionOptions {
                Schema = _schema, Query = query.Query };

            try
            {
                var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

                if (result.Errors?.Count > 0)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}