using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureDemo.Model;
using Newtonsoft.Json;

namespace AzureDemo
{
    public static class Saver
    {
        [FunctionName("Saver")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("customers", Connection= "AzureWebJobsStorage")] IAsyncCollector<CustomerEntity> customerQueue,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function Saver processed a request.");
        
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();	
            log.LogInformation($"request received: {requestBody}");
            CustomerEntity customer = JsonConvert.DeserializeObject<CustomerEntity>(requestBody);
            log.LogInformation("calling code to queue customer details ...");
            await customerQueue.AddAsync(customer);
                      
           return customer != null
            ? (ActionResult) new OkObjectResult($"Customer was saved successfully")
            : new BadRequestObjectResult("Customer needs to have a value");
        }
    }
}
