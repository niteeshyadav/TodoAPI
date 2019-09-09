using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using FluentAssertions;

namespace TodoApi.Common
{
    public class TestCustomExceptionMiddleware
    {
        [Fact]
        public async Task WhenAGenericExceptionIsRaised_CustomExceptionMiddlewareShouldHandleItToDefaultErrorResponseAndLoggerCalled()
        {
            // Arrange

            var loggerMock = Substitute.For<ILogger<CustomExceptionMiddleware>>();
            
            var middleware = new CustomExceptionMiddleware((innerHttpContext) =>
            {
                throw new Exception("Oooops error!");
            }, loggerMock);

            HttpContext context = new DefaultHttpContext();

            // Initialize response body
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            // set the position to beginning before reading
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Read the response
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var objResponse = JsonConvert.DeserializeObject<Exception>(streamText);

            //Assert
            objResponse
            .Should()
            .BeEquivalentTo(new { Message = "Unhandled error", Errors = new List<string>(), Code = "00009" });

            context.Response.StatusCode
            .Should()
            .Be((int)HttpStatusCode.InternalServerError);

            loggerMock.Received(1);
        }
    }
}
