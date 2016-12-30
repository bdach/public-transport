using System;
using System.Security.Claims;
using Microsoft.Owin.Security;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer;

namespace PublicTransport.WebAPI.Identity
{
    public class LoginProvider
    {
        private static readonly LoginService LoginService = new LoginService();

        public bool ValidateCredentials(LoginData loginData, out ClaimsIdentity identity)
        {
            var isValid = LoginService.ValidateCredentials(loginData);
            if (isValid)
            {
                identity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, loginData.UserName));
            }
            else
            {
                identity = null;
            }

            return isValid;
        }

        public UserInfo CreateUserInfo(LoginData loginData, ClaimsIdentity identity)
        {
            AuthenticationTicket ticket;
            DateTimeOffset ticketExpirationDate;
            CreateAuthenticationTicket(identity, out ticket, out ticketExpirationDate);

            LoginService.UpdateUserToken(loginData.UserName, Startup.OAuthOptions.AccessTokenFormat.Protect(ticket));
            return LoginService.GetUserInfoByUserName(loginData.UserName);
        }

        private static void CreateAuthenticationTicket(ClaimsIdentity identity, out AuthenticationTicket ticket, out DateTimeOffset ticketExpirationDate)
        {
            ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = DateTime.UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticketExpirationDate = currentUtc.AddHours(12);
            ticket.Properties.ExpiresUtc = ticketExpirationDate;
        }

        public static UserInfo RestoreSession(string token)
        {
            return LoginService.GetUserInfoByToken(token);
        }
    }
}
