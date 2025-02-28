using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Errors.Response;
using System.Net;

namespace EventManagment.Apis.MiddleWares
{
    public class ExeptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExeptionHandlerMiddleware(RequestDelegate next, ILogger<ExeptionHandlerMiddleware> loggerFactory, IHostEnvironment env)
        {
            _next = next;
            _logger = loggerFactory;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == (int)HttpStatusCode.MethodNotAllowed)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";
                    var respnse = new ApiResponse((int)HttpStatusCode.Unauthorized, $"You Are Not Authorized");
                    await httpContext.Response.WriteAsync(respnse.ToString());
                }
            }
            catch (Exception ex)
            {

                if (_env.IsDevelopment())
                {
                    _logger.LogError(ex, ex.Message);
                }
                else
                {
                    // production mode
                    // log exeption details t (file | text)

                }
                await HandleExceptionAsync(httpContext, ex);

            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            ApiResponse response;
            switch (ex)
            {
                case NotFoundExeption:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse((int)HttpStatusCode.NotFound, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case ValidationExeption validationExeption:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiValidationErrorResponse(ex.Message) { Erroes = validationExeption.Errors };
                    await httpContext.Response.WriteAsync(response.ToString());

                    break;
                case BadRequestExeption:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(400, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());

                    break;
                case UnAuthorizedExeption:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(401, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());

                    break;
                default:
                    response = _env.IsDevelopment() ? new ApiExeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
                        : new ApiExeptionResponse((int)HttpStatusCode.InternalServerError);



                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";

                    await httpContext.Response.WriteAsync(response.ToString());
                    break;


            }

        }
    }
}
