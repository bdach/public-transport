using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace PublicTransport.WebAPI.Controllers
{
    public class TripController : ApiController
    {
        private static readonly TripRepository TripRepository = new TripRepository(new PublicTransportContext());
        private static readonly TripConverter TripConverter = new TripConverter();

        [HttpPost, Route("trip/search")]
        public IHttpActionResult Filter(HttpRequestMessage request, [FromBody] RouteSearchFilter filter)
        {
            if (filter != null)
            {
                var trips = TripRepository.FindTrips(filter);
                var dtos = trips.Select(TripConverter.GetDto).ToList();
                return Ok(dtos);
            }
            return BadRequest("No filter was supplied");
        }
    }
}
