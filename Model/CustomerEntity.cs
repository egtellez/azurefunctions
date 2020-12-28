using Microsoft.WindowsAzure.Storage.Table;

namespace AzureDemo.Model{
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity()
        {

        }
        public string Name {get; set;}
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        public int Age { get; set; }
    }
}