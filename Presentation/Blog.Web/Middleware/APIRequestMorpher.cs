using Blog.Web.Models.ApiModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Web.Middleware
{
    public class APIRequestMorpher : IMiddleware
    {
        public APIRequestMorpher()
        {
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isAPI = context.Request.Path.Value.Contains("api") && !context.Request.Path.Value.Contains("SearchBlog");

            if (isAPI)
            {
                List<string> req = await GetListOfStringsFromStream(context.Request.Body);
                ActionApiModel orgRequest = JsonConvert.DeserializeObject<ActionApiModel>(req.FirstOrDefault());
                orgRequest.User = new Entities.UserContextEntity();
                orgRequest.User.UserName = context.User.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                orgRequest.User.UserPicPath = context.User.Claims.Where(c => c.Type == "PicPath").Select(c => c.Value).FirstOrDefault();
                orgRequest.User.UserId = context.User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).FirstOrDefault();
                byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(orgRequest));
                context.Request.Body = new MemoryStream(byteArray);
            }

            await next(context);

            // var response = await FormatResponse(context.Response);

            //TODO: Save log to chosen datastore

            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.

        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            // request.EnableRewind();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }

        private static async Task<List<string>> GetListOfStringsFromStream(Stream requestBody)
        {
            // Build up the request body in a string builder.
            StringBuilder builder = new StringBuilder();

            // Rent a shared buffer to write the request body into.
            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);

            while (true)
            {
                var bytesRemaining = await requestBody.ReadAsync(buffer, offset: 0, buffer.Length);
                if (bytesRemaining == 0)
                {
                    break;
                }

                // Append the encoded string into the string builder.
                var encodedString = Encoding.UTF8.GetString(buffer, 0, bytesRemaining);
                builder.Append(encodedString);
            }

            ArrayPool<byte>.Shared.Return(buffer);

            var entireRequestBody = builder.ToString();

            // Split on \n in the string.
            return new List<string>(entireRequestBody.Split("\n"));
        }
    }
}
