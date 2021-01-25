using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw3_6.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request != null)
            {
                string path = context.Request.Path;
                string method = context.Request.Method;
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";
                string log;

                using (StreamReader reader
                   = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
                // zapis do pliku/bazy
                log = "Path: " + path + " \n" +
                  "Method: " + method + " \n" +
                  "QueryString: " + queryString + " \n" +
                  "Body: " + bodyStr + " \n" +
                  " " + " \n";

                File.AppendAllText("requestsLog.txt", log);
            }

            if(_next!=null) await _next(context);
        }
    }
}
