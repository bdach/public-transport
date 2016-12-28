using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using PublicTransport.Services.DataTransfer;
using PublicTransport.WebAPI.Identity;

namespace PublicTransport.WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        private static readonly LoginProvider LoginProvider = new LoginProvider();

        [HttpPost, Route("token")]
        public IHttpActionResult Login(HttpRequestMessage request, [FromBody]LoginData loginData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ClaimsIdentity identity;
            if (!LoginProvider.ValidateCredentials(loginData, out identity))
            {
                return Unauthorized();
            }

            var userInfo = LoginProvider.CreateUserInfo(loginData, identity);
            return Ok(userInfo);
        }
    }
}
