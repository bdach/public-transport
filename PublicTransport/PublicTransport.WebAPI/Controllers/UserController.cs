using System.Net.Http;
using System.Web.Http;
using PublicTransport.Domain.Context;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        private static readonly UserRepository UserRepository = new UserRepository(new PublicTransportContext());
        private static readonly UserConverter UserConverter = new UserConverter();

        [HttpPost]
        public IHttpActionResult Register(HttpRequestMessage request, [FromBody]UserDto user)
        {
            if (user != null)
            {
                try
                {
                    UserRepository.Create(UserConverter.GetEntity(user));
                    return Ok();
                }
                catch (UserAlreadyExistsException)
                {
                    return BadRequest("Provided username is already taken");
                }
            }
            return BadRequest("Provided user model is not valid");
        }
    }
}
