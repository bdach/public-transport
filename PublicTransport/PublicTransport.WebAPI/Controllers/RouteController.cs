﻿using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.WebAPI.Controllers
{
    public class RouteController : ApiController
    {
        private static readonly RouteRepository RouteRepository = new RouteRepository(new PublicTransportContext());

        [HttpPost]
        public IHttpActionResult Filter(HttpRequestMessage request, [FromBody]RouteFilter filter)
        {
            if (filter != null && filter.IsValid)
            {
                var routes = RouteRepository.FilterRoutes(filter);
                var routesDto = routes.Select(r => new RouteInfo(r));
                return Ok(routesDto);
            }
            return BadRequest("The supplied filter is invalid");
        }
    }
}