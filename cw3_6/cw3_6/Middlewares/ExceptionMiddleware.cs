using cw3_6.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3_6.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext)
            }catch(Exception exc)
            {
                await HandleExceptionAsync(httpContext, exc);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception exc)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return httpContext.Response.WriteAsync(new ErrorDetails 
            { 
                StatusCode=(int)StatusCodes.Status500InternalServerError, Message = "Wystąpił błąd"
            }.ToString());
        }
    }
}
