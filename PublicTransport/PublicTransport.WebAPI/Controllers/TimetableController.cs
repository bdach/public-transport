using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.WebAPI.Controllers
{
    public class TimetableController : ApiController
    {
        private static readonly StopTimeRepository StopTimeRepository = new StopTimeRepository(new PublicTransportContext());

        [HttpGet, Route("timetable/stop/{id}")]
        public IHttpActionResult Stop(HttpRequestMessage request, int id)
        {
            var stopTimes = StopTimeRepository.GetFullTimetableByStopId(id)
                .ToDictionary(kv => new RouteInfo(kv.Key),
                              kv => kv.Value.Select(st => new TimetableEntry(st)).ToList())
                .ToList();
            return Ok(stopTimes);
        }
        
        [HttpGet, Route("timetable/route/{id}")]
        public IHttpActionResult Route(HttpRequestMessage request, int id)
        {
            var stopTimes = StopTimeRepository
                .GetFullTimetableByRouteId(id)
                .ToDictionary(kv => new StopInfo(kv.Key),
                              kv => kv.Value.Select(st => new TimetableEntry(st)).ToList())
                .ToList();
            return Ok(stopTimes);
        }
    }
}
