using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using AzureDemo.Model;
public class TableOperator
{
     private readonly CloudStorageAccount cloudStorageAccount;
    private readonly CloudTable table;
    public TableOperator(string tableName)
    {
        var credentials = new StorageCredentials("ximenaazuredemostorage1", "28sADo9xylhj5QbOLEWdPsom+XYSzf/8moDBFkSxunvnkkulROZMWkjb5lMsnwyTirq1+j0+9Cfi5mgw4w5zxA==");
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
