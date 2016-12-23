using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.WebAPI.Controllers
{
    public class TimetableController : ApiController
    {
        private static readonly StopTimeRepository StopTimeRepository = new StopTimeRepository(new PublicTransportContext());
        private static readonly StopTimeConverter StopTimeConverter = new StopTimeConverter();
        private static readonly RouteConverter RouteConverter = new RouteConverter();
        private static readonly StopConverter StopConverter = new StopConverter();

        [HttpGet, Route("timetable/stop/{id}")]
        public IHttpActionResult Stop(HttpRequestMessage request, int id)
        {
            var stopTimes = StopTimeRepository.GetFullTimetableByStopId(id)
                .ToDictionary(kv => RouteConverter.GetDto(kv.Key),
                              kv => kv.Value.Select(st => StopTimeConverter.GetDto(st)).ToList())
                .ToList();
            return Ok(stopTimes);
        }
        
        [HttpGet, Route("timetable/route/{id}")]
        public IHttpActionResult Route(HttpRequestMessage request, int id)
        {
            var stopTimes = StopTimeRepository
                .GetFullTimetableByRouteId(id)
                .ToDictionary(kv => StopConverter.GetDto(kv.Key),
                              kv => kv.Value.Select(st => StopTimeConverter.GetDto(st)).ToList())
                .ToList();
            return Ok(stopTimes);
        }
    }
}
