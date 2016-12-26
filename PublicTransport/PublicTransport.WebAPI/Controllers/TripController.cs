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
        private static readonly TripRepository TripRepository = new TripRepository(new PublicTransportContext());

        [HttpPost]
        public IHttpActionResult Search(HttpRequestMessage request, [FromBody]RouteSearchFilter filter)
        {
            if (filter != null && filter.IsValid)
            {
                var trips = TripRepository.FindTrips(filter);
                var tripsDto = trips.Select(t => new TripInfo(t)).ToList();
                return Ok(tripsDto);
            }
            return BadRequest("The supplied filter is invalid");
        }
    }
}
