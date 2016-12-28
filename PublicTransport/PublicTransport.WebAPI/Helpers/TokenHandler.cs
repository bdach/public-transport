using System.Linq;
using System.Net.Http;
using PublicTransport.Domain.Context;

namespace PublicTransport.WebAPI.Helpers
{
    public class TokenHandler
    {
        public static string GetTokenFromHeader(HttpRequestMessage request)
        {
            var headerValues = request.Headers.GetValues("Authorization");
            return headerValues.FirstOrDefault().Substring(7);
        }

        public static bool IsUserAuthorized(string userName, string token)
        {
            using (var db = new PublicTransportContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == userName);
                return user != null && user.LatestToken == token;
            }
        }
    }
}