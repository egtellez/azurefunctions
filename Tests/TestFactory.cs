using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using AzureDemo.Model;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Functions.Tests
{
    public class TestFactory
    {

        public static HttpRequest CreateHttpRequest(CustomerEntity customer)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(customer));
            MemoryStream stream = new MemoryStream(byteArray);
            request.Body = stream;
            return request;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}