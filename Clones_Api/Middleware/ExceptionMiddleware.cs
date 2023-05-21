using Models.Dtos;
using Newtonsoft.Json;
using System.Net;

namespace Clones_Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception error)
            {

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new BaseResponse<string>().Error(error.Message);
                switch (error)
                {
                    case UnauthorizedAccessException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        responseModel.Code = (int)HttpStatusCode.Unauthorized;
                        responseModel.Message = e.Message;
                        break;
                    case ArgumentOutOfRangeException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Code = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = e.Message;
                        break;
                    case ArgumentException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Code = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = e.Message;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = "Internal Server Error. Please Try Again Later.";
                        break;
                }
                var result = JsonConvert.SerializeObject(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}