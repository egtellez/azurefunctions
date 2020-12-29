using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using AzureDemo.Model;

public interface ITableOperator{
    Task<TableResult> InsertAsync(CustomerEntity customerEntity);
}