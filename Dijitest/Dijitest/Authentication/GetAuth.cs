using System.Security.Claims;
using System.Threading.Tasks;
using Dijitest.Models.DataBase;
using Microsoft.Owin.Security.OAuth;

namespace Dijitest.Authentication 
{
    public class GetAuth : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" }); 
            
            Get_LoginResult Users = Dijitest.Models.Token.Token.GetToken(context.UserName, context.Password);
            if (Users != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("ID", Users.ID.ToString()));
                identity.AddClaim(new Claim("name", context.UserName));
                context.Validated(identity);
            }
            else
            {
                context.SetError("Kullanıcı Bilgisi Hatalı !", "Lütfen kontrol ediniz.");
            }


        }
    }
}