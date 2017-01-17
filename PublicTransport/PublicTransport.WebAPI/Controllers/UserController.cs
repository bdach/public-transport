using System.Linq;
using System.Net.Http;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Helpers;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        private readonly UserRepository _userRepository;
        private static readonly UserConverter UserConverter = new UserConverter();
        private readonly ILoginService _loginService;

        public UserController(PublicTransportContext db, ILoginService loginService)
        {
            _userRepository = new UserRepository(db);
            _loginService = loginService;
        }

        [HttpPost, Route("user/register")]
        public IHttpActionResult Register(HttpRequestMessage request, [FromBody]UserDto user)
        {
            if (user != null)
            {
                try
                {
                    _userRepository.Create(UserConverter.GetEntity(user));
                    return Ok();
                }
                catch (UserAlreadyExistsException)
                {
                    return BadRequest("Provided username is already taken");
                }
            }
            return BadRequest("Provided user model is not valid");
        }

        [Authorize]
        [HttpPost, Route("user/changepassword")]
        public IHttpActionResult ChangePassword(HttpRequestMessage request, [FromBody]PasswordChangeData data)
        {
            if (data != null)
            {
                var token = TokenHandler.GetTokenFromHeader(request);
                if (!TokenHandler.IsUserAuthorized(data.UserName, token))
                {
                    return Unauthorized();
                }

                try
                {
                    _loginService.RequestPasswordChange(data);
                    return Ok();
                }
                catch (InvalidCredentialsException)
                {
                    return BadRequest("Provided username or old password is invalid");
                }
            }
            return BadRequest("Provided data model is not valid");
        }

        [Authorize]
        [HttpGet, Route("user/favouritestops")]
        public IHttpActionResult GetFavouriteStops(HttpRequestMessage request)
        {
            var token = TokenHandler.GetTokenFromHeader(request);
            var username = TokenHandler.GetUserNameByToken(token);

            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Provided username is invalid");
            }

            var stops = _userRepository.GetFavouriteStopsByUserName(username);
            var stopsDto = stops.Select(s => new StopInfo(s)).ToList();
            return Ok(stopsDto);
        }

        [Authorize]
        [HttpPost, Route("user/favouritestops")]
        public IHttpActionResult UpdateFavouriteStops(HttpRequestMessage request, [FromBody]FavouriteInfo data)
        {
            var token = TokenHandler.GetTokenFromHeader(request);
            if (!TokenHandler.IsUserAuthorized(data.UserName, token))
            {
                return Unauthorized();
            }

            var result = _userRepository.UpdateFavouriteStops(data.Changes, data.UserName);
            var resultDto = result.Select(s => new StopInfo(s)).ToList();
            return Ok(resultDto);
        }

        [Authorize]
        [HttpGet, Route("user/favouriteroutes")]
        public IHttpActionResult GetFavouriteRoutes(HttpRequestMessage request)
        {
            var token = TokenHandler.GetTokenFromHeader(request);
            var username = TokenHandler.GetUserNameByToken(token);

            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Provided username is invalid");
            }

            var routes = _userRepository.GetFavouriteRoutesByUserName(username);
            var routesDto = routes.Select(r => new RouteInfo(r)).ToList();
            return Ok(routesDto);
        }

        [Authorize]
        [HttpPost, Route("user/favouriteroutes")]
        public IHttpActionResult UpdateFavouriteRoutes(HttpRequestMessage request, [FromBody]FavouriteInfo data)
        {
            var token = TokenHandler.GetTokenFromHeader(request);
            if (!TokenHandler.IsUserAuthorized(data.UserName, token))
            {
                return Unauthorized();
            }

            var result = _userRepository.UpdateFavouriteRoutes(data.Changes, data.UserName);
            var resultDto = result.Select(s => new RouteInfo(s)).ToList();
            return Ok(resultDto);
        }
    }
}
