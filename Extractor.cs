using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AzureDemo.Model;
using Microsoft.WindowsAzure.Storage.Table;
using DemoExceptions;
using System;

namespace AzureDemo
{
    public static class Extractor
    {
        [FunctionName("Extractor")]
        public static void Run([QueueTrigger("customers", Connection = "ximenaazuredemostorage1_STORAGE")]CustomerEntity customer, ILogger log)
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
            catch(AggregateException ex)
            {
                if(ex.InnerExceptions[0].GetType() == typeof(DuplicateRowKeyException)){
                    log.LogError("Couldn´t insert row into table because row key is duplicate. {customer}");
                }
                else
                {
                    log.LogError(ex.Message);
                }
            }            
        }
    }
}
