using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.WebAPI.Controllers
{
    public class StopController : ApiController
    {
        private static readonly StopRepository StopRepository = new StopRepository(new PublicTransportContext());

        [HttpPost]
        public IHttpActionResult Filter(HttpRequestMessage request, [FromBody]StopFilter filter)
        {
            if (filter != null && filter.IsValid)
            {
                var stops = StopRepository.FilterStops(filter);
                var stopsDto = stops.Select(s => new StopInfo(s));
                return Ok(stopsDto);
            }
            return BadRequest("The supplied filter is invalid");
        }
    }
}
