using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AzureDemo.Model;
using Helpers;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDemo
{
    public static class Extractor
    {
        [FunctionName("Extractor")]
        public static void Run([QueueTrigger("customers", Connection = "AzureWebJobsStorage")]CustomerEntity customer, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {customer}");         
            var tableOperator = new TableOperator("customers");
            customer.PartitionKey = "customers";
            customer.RowKey = customer.Name;
            try
            {
                TableResult result = tableOperator.InsertAsync(customer).Result;
                log.LogInformation($"Table result {result.Result.ToString()}");
            }
            catch(DuplicateRowKeyException)
            {
                log.LogError("Couldn´t insert row into table because row key is duplicate. {customer}");
            }            
        }
    }
}
