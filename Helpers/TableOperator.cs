using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using AzureDemo.Model;
using DemoExceptions;
using System;
using Microsoft.Extensions.Configuration;

public class TableOperator : ITableOperator
{
    private readonly CloudStorageAccount cloudStorageAccount;
    private readonly CloudTable table;
    public TableOperator(string tableName)
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
        
        var accountName = config["ximenaazuredemostorage1_accountname"];
        var accountKey = config["ximenaazuredemostorage1_accountkey"];

        var credentials = new StorageCredentials(accountName, accountKey);
        cloudStorageAccount = new CloudStorageAccount(credentials, useHttps: true);
        var tableClient = cloudStorageAccount.CreateCloudTableClient();
        table = tableClient.GetTableReference(tableName);
    }

    public async Task<TableResult> InsertAsync(CustomerEntity customerEntity)
    {
        if(await table.ExistsAsync())
        {
            try{
                return await table.ExecuteAsync(TableOperation.Insert(customerEntity));
            }
            catch(StorageException ex)
            {
                if(ex.Message.Contains("Conflict")){
                    throw new DuplicateRowKeyException();
                }
            }
        } 

        return null;
    }

}
