using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.WebAPI.Controllers
{
    public class StopController : ApiController
    {
        private static readonly StopRepository StopRepository = new StopRepository(new PublicTransportContext());
        private static readonly StopConverter StopConverter = new StopConverter();

        [HttpPost]
        public IHttpActionResult Filter(HttpRequestMessage request, [FromBody]StopFilter filter)
        {
            if (filter != null && filter.IsValid)
            {
                var stops = StopRepository.FilterStops(filter);
                var stopsDto = stops.Select(s => StopConverter.GetDto(s));
                return Ok(stopsDto);
            }
            return BadRequest("The supplied filter is invalid");
        }
    }
}
