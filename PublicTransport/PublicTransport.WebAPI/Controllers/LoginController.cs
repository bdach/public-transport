using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using PublicTransport.WebAPI.Helpers;
using PublicTransport.WebAPI.Identity;

namespace PublicTransport.WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        private readonly LoginProvider _loginProvider;

        public LoginController(LoginProvider loginProvider)
        {
            _loginProvider = loginProvider;
        }

        [HttpPost, Route("token")]
        public IHttpActionResult Login(HttpRequestMessage request, [FromBody]LoginData loginData)
        {
            if (loginData != null)
            {
                ClaimsIdentity identity;
                if (!_loginProvider.ValidateCredentials(loginData, out identity))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = "Invalid username or password" }));
                }

                var userInfo = _loginProvider.CreateUserInfo(loginData, identity);
                return Ok(userInfo);
            }
            return BadRequest("Provided login data model is not valid");
        }

        [Authorize]
        [HttpGet, Route("session")]
        public IHttpActionResult Session(HttpRequestMessage request)
        {
            UserInfo userInfo;
            try
            {
                var token = TokenHandler.GetTokenFromHeader(request);
                userInfo = _loginProvider.RestoreSession(token);
            }
            catch (EntryNotFoundException)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Unauthorized, new { Message = "Please log in again" }));
            }

            return Ok(userInfo);
        }
    }
}
