using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;
using CatalogService.Domain.Models;
using MMLib.SwaggerForOcelot.Aggregates;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace OcelotApiGateway.Aggregators
{
    [AggregateResponse("Item details", typeof(DownstreamResponse))]
    public class ItemDetailsAggegator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var item = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<Item>();
            var details = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<Dictionary<string, string>>();

            var result = new Dictionary<string, dynamic>()
            {
                { "item", item },
                { "details", details}
            };
            var jsonString = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

            var stringContent = new StringContent(jsonString)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
        }
    }
}
