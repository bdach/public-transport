using System.Net.Http;
using System.Web.Http;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        private static readonly LoginService LoginService = new LoginService();

        [HttpPost, Route("token")]
        public IHttpActionResult Login(HttpRequestMessage request, [FromBody]LoginData loginData)
        {
            if (loginData != null)
            {
                try
                {
                    var userInfo = LoginService.RequestLogin(loginData);
                    // wygenerowanie tokenu dla usera
                    return Ok(userInfo);
                }
                catch (InvalidCredentialsException)
                {
                    return Unauthorized();
                }
            }
            return BadRequest("Provided login data model is not valid");
        }
    }
}
