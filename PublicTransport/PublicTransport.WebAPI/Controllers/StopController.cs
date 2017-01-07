using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.WebAPI.Controllers
{
    public class StopController : ApiController
    {
        private readonly IStopRepository _stopRepository;

        public StopController(IStopRepository stopRepository)
        {
            _stopRepository = stopRepository;
        }
        
        [HttpPost]
        public IHttpActionResult Filter(HttpRequestMessage request, [FromBody]StopFilter filter)
        {
            if (filter != null && filter.IsValid)
            {
                var stops = _stopRepository.FilterStops(filter);
                var stopsDto = stops.Select(s => new StopInfo(s));
                return Ok(stopsDto);
            }
            return BadRequest("The supplied filter is invalid");
        }
    }
}
