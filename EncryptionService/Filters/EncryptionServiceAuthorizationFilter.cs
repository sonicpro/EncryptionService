using Encryption.Interfaces;
using Encryption.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Threading.Tasks;

namespace Encryption.Filters
{
    public class EncryptionServiceAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ITokenValidator tokenValidator;

        public EncryptionServiceAuthorizationFilter(ITokenValidator validator)
        {
            tokenValidator = validator;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            var authorization = request.Headers["Authorization"];
            ObjectResult fallaciousResult = null;

            if (string.IsNullOrEmpty(authorization))
            {
                fallaciousResult = new ObjectResult("Missing credentials")
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }

            // In real app the Authorization header will contain Jwt token with at payload containing claims provided
            // by the authorization endpoint party.
            var validationResult = await tokenValidator.ValidateGrant(new AccessToken { Token = authorization });

            if (validationResult.Type == AuthResultType.Expired)
            {
                fallaciousResult = new ObjectResult("Session timeout")
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            
            if (validationResult.Type != AuthResultType.Ok)
            {
                fallaciousResult = new ObjectResult("Invalid credentials")
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }

            if (fallaciousResult != null)
            {
                context.Result = fallaciousResult;
                return;
            }
        }
    }
}
