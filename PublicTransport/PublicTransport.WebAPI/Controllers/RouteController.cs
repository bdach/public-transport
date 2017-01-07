using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.WebAPI.Controllers
{
    public class RouteController : ApiController
    {
        private readonly IRouteRepository _routeRepository;

        public RouteController(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        [HttpPost]
        public IHttpActionResult Filter(HttpRequestMessage request, [FromBody]RouteFilter filter)
        {
            if (filter != null && filter.IsValid)
            {
                var routes = _routeRepository.FilterRoutes(filter);
                var routesDto = routes.Select(r => new RouteInfo(r));
                return Ok(routesDto);
            }
            return BadRequest("The supplied filter is invalid");
        }
    }
}
