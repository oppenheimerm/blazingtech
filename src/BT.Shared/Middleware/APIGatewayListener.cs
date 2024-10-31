using Microsoft.AspNetCore.Http;

namespace BT.Shared.Middleware
{
    /// <summary>
    /// Prevent client access the API service directly.  We need all our clients
    /// gaing access via ou API gatway.
    /// </summary>
    public class APIGatewayListener(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //  Extract specific header for the request
            var signedHeader = context.Request.Headers[AppConstants.ApiGateway];

            // If null, request is not coming from APIGatway
            if (signedHeader.FirstOrDefault() is null)
            {
                // Client is accessing service directly(Which we DONT want)
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync(AppConstants.ServiceIsUnavailable503);
                return;
            }
            else
            {
                // Excute next middleware
                await next(context);
            }
        }
    }
}
