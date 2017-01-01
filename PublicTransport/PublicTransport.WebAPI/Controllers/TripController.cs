using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.WebAPI.Controllers
{
    public class TripController : ApiController
    {
        private readonly TripRepository _tripRepository;

        public TripController(PublicTransportContext db)
        {
            _tripRepository = new TripRepository(db);
        }

        [HttpPost, Route("trip/search")]
        public IHttpActionResult Search(HttpRequestMessage request, [FromBody]RouteSearchFilter filter)
        {
            if (filter != null && filter.IsValid)
            {
                var trips = _tripRepository.FindTrips(filter);
                var tripsDto = trips.Select(t => new TripInfo(t)).ToList();
                return Ok(tripsDto);
            }
            return BadRequest("The supplied filter is invalid");
        }

        [HttpPost, Route("trip/mapData")]
        public IHttpActionResult MapData(HttpRequestMessage request, [FromBody] TripSegmentFilter filter)
        {
            if (filter == null) return BadRequest("No filter was supplied");
            if (!filter.IsValid) return BadRequest("The supplied filter is invalid");
            var stops = _tripRepository.GetTripSegment(filter);
            var stopsDto = stops.Where(st => st.Shape != null).Select(st => new MapMarker(st)).ToList();
            return Ok(stopsDto);
        }
    }
}
