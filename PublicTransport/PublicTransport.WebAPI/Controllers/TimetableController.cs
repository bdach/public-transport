using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.WebAPI.Controllers
{
    public class TimetableController : ApiController
    {
        private static readonly StopTimeRepository StopTimeRepository = new StopTimeRepository(new PublicTransportContext());
        private static readonly StopTimeConverter StopTimeConverter = new StopTimeConverter();
        private static readonly RouteConverter RouteConverter = new RouteConverter();

        [HttpGet, Route("timetable/stop/{id}")]
        public IHttpActionResult Stop(HttpRequestMessage request, int id)
        {
            var stopTimes = StopTimeRepository.GetFullTimetableByStopId(id)
                .ToDictionary(kv => RouteConverter.GetDto(kv.Key),
                              kv => kv.Value.Select(st => StopTimeConverter.GetDto(st)).ToList());
            return Ok(stopTimes);
        }

        //[HttpGet]
        //public IHttpActionResult Stop(HttpRequestMessage request, int id = 0)
        //{
        //    if (id > 0)
        //    {
        //        var stopTimes = StopTimeRepository.GetFullTimetableByStopId(id)
        //            .ToDictionary(kv => RouteConverter.GetDto(kv.Key),
        //                          kv => kv.Value.Select(st => StopTimeConverter.GetDto(st)).ToList());
        //        return Ok(stopTimes);
        //    }
        //    return BadRequest("The supplied id is invalid");
        //}

        [HttpPost]
        //[HttpGet, Route("timetable/route/{id}")] // zamienić na taki i wykorzystać GetFullTimetableByRouteId(id)
        public IHttpActionResult Route(HttpRequestMessage request, [FromBody]StopTimeFilter filter)
        {
            var stopTimes = StopTimeRepository
                .GetRouteTimetableByStopId(filter)
                .Select(StopTimeConverter.GetDto).ToList();
            return Ok(stopTimes);
        }
    }
}
