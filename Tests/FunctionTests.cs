using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using AzureDemo.Model;
using AzureDemo;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;

namespace Functions.Tests
{
    public class FunctionsTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        private CustomerEntity GetDefaultCustomer()
        {
            var customer = new CustomerEntity{
                Name = "Customer1",
                Email = "customer1@azure.com",
                Address = "Nowhere 35 China",
                PhoneNumber = "333445556",
                Age = 20
            };
            return customer;
        }

        [Fact]
        public async void ShouldReturnCustomerSuccessMessageWhenSaverTriggered()
        {
            
            CustomerEntity customer = GetDefaultCustomer();
            IAsyncCollector<CustomerEntity> asyncCollector = new Mock<IAsyncCollector<CustomerEntity>>().Object;
            var request = TestFactory.CreateHttpRequest(customer);
            var response = (OkObjectResult)await Saver.Run(request, asyncCollector, logger);
            Assert.Equal("Customer was saved successfully", response.Value);
        }
    }
}