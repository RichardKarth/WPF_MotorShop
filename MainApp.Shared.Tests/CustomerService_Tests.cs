

using Castle.Core.Resource;
using MainApp.Shared.Enums;
using MainApp.Shared.Interfaces;
using MainApp.Shared.Models;
using MainApp.Shared.Services;
using Moq;

namespace MainApp.Shared.Tests;

public class CustomerService_Tests
{
    private readonly ICustomerService _customerService;
    private readonly Mock<IFileService> _fileServiceMock;
    


    public CustomerService_Tests()
    {
        _fileServiceMock = new Mock<IFileService>();
        var customerList = new List<Customer>
        {
            new Customer { Id = "123", FirstName = "Richard", Email = "richard@domain.se" }
        };
        _customerService = new CustomerService(_fileServiceMock.Object,customerList);
       
    }

    [Fact]
    public void SaveCustomer_ShouldReturnSuccess_WhenCustomerIsSaved()
    {
        //Arrange
        var customer = new Customer { FirstName = "Richard", Email = "richard@domain.se" };
        //Act
        var result = _customerService.SaveCustomer(customer);
        //Assert
        Assert.Equal(ServiceResponse.Success, result);
    }
    [Fact]
    public void DeleteCustomer_ShouldReturnSuccess_WhenCustomerIsDeletedFromFile()
    {
        //Arrange
        var customer = new Customer { FirstName = "Richard", Email = "richard@domain.se", Id = Guid.NewGuid().ToString() };
        string filePath = "customer.json";

        //Act

        //Mocks both methods DeleteFromFile uses
        _fileServiceMock.Setup(x => x.GetFromFile()).Returns("[{\"FirstName\":\"Richard\", \"Email\":\"richard@domain.se\", \"Id\":\"" + customer.Id + "\"}]");
        _fileServiceMock.Setup(x => x.SaveToFile(It.IsAny<string>())).Returns(ServiceResponse.Success);


        var deleteResult = _customerService.DeleteCustomer(customer.Id);
        //Assert
        Assert.Equal(ServiceResponse.Success, deleteResult);
    }
    [Fact]
    public void UpdateCustomer_ShouldReturnSuccess_WhenCustomerIsUpdated()
    {
        //Arrange
        var customer = new Customer { FirstName = "Richard", Email = "richard@domain.se", Id = "123"};

        //Act
        _fileServiceMock.Setup(x => x.SaveToFile(It.IsAny<string>())).Returns(ServiceResponse.Success);

        var updateResult = _customerService.UpdateCustomer(customer);

        //Assert
        Assert.Equal(ServiceResponse.Success, updateResult);
    }

    [Fact]
    public void SaveToFile_ShouldReturnSuccess_WhenContentisTwo()
    {
        //Arrange
        var customer1 = new Customer { FirstName = "A", Email = "A@domain.se", Id = Guid.NewGuid().ToString() };
        var customer2 = new Customer { FirstName = "B", Email = "B@domain.se", Id = Guid.NewGuid().ToString() };


        //Act
        var result1 = _customerService.SaveCustomer(customer1);
        var result2 = _customerService.SaveCustomer(customer2);
        _fileServiceMock.Setup(x => x.GetFromFile()).Returns("[{\"FirstName\":\"A\", \"Email\":\"A@domain.se\", \"Id\":\"" + customer1.Id + "\"}, {\"FirstName\":\"B\", \"Email\":\"B@domain.se\", \"Id\":\"" + customer2.Id + "\"}]");
        

        //Assert
        var customers = _customerService.GetCustomer().ToList();
        Assert.Equal(2, customers.Count);
    }


    [Fact]
    public void SaveToFile_ShouldReturnSuccess_WhenContentisAdded()
    {
        //Arrange 
        string content = "content";

        _fileServiceMock.Setup(x => x.SaveToFile(content)).Returns(ServiceResponse.Success);

        //Act
        var result = _fileServiceMock.Object.SaveToFile(content);

        //Assert
        Assert.Equal(ServiceResponse.Success, result);
    }
    [Fact]
    public void GetFromFile_ShouldReturnSuccess_WhenCustomersAreRetrived()
    {
        //Arrange
        string filePath = "testfile.txt";
        string expectedContent = "File content";

        _fileServiceMock.Setup(x => x.SaveToFile(filePath)).Returns(ServiceResponse.Success);
        _fileServiceMock.Setup(x => x.GetFromFile()).Returns(expectedContent);
        
        //Act
        var result = _fileServiceMock.Object.GetFromFile();

        //Assert
        Assert.Equal(expectedContent, result);
    }

}