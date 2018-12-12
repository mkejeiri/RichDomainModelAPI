using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Api.Utils
{
    //This will be a single processor middleware that needs to be incorporated to the pipeline.

    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }
        //this will introduce a global try catch statement to catch all exceptions from all pieces of middleware down to execution stack        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        //single processor for all exceptions
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //log the exception here

            //serialize it
            //string result = JsonConvert.SerializeObject(new { error = exception.Message });
            string result = JsonConvert.SerializeObject(Envelope.Error(exception.Message));
            //fill the statuscode of the response
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            //write the json string in the response
            return context.Response.WriteAsync(result);
        }
    }
}
