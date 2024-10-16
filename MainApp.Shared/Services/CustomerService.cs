
using MainApp.Shared.Enums;
using MainApp.Shared.Interfaces;
using MainApp.Shared.Models;
using Newtonsoft.Json;

namespace MainApp.Shared.Services;

public class CustomerService : ICustomerService
{
    
    private List<Customer> _customerList;
    private readonly IFileService _fileService;

    public CustomerService(IFileService fileService)
    {
        _fileService = fileService;
        _customerList = [];
    }
    public ServiceResponse SaveCustomer(Customer customer)
    {
        _customerList.Add(customer);
        var result = _fileService.SaveToFile(JsonConvert.SerializeObject(_customerList, Formatting.Indented));
        return ServiceResponse.Success;
    }

    public ServiceResponse UpdateCustomer(Customer customer)
    {
        var existingUser = _customerList.FirstOrDefault(c => c.Id == customer.Id);
        try
        {
            if (existingUser == customer)
            {
                existingUser = customer;
                var result = _fileService.SaveToFile(JsonConvert.SerializeObject(_customerList, Formatting.Indented));
                return ServiceResponse.Success;
            }
            else
            {
                return ServiceResponse.NotFound;
            }
        }
        catch
        {
            return ServiceResponse.Error;
        }

    }
    public IEnumerable<Customer> GetCustomer()
    {
        var listContent = _fileService.GetFromFile();
        var tempList = JsonConvert.DeserializeObject<List<Customer>>(listContent);
        if (tempList != null && tempList.Count > 0)
        {
            _customerList = tempList;
        }
        return _customerList;
    }
    public ServiceResponse DeleteCustomer(string id)
    {
        _customerList = GetCustomer().ToList();
        var customer = _customerList.FirstOrDefault(c => c.Id == id);
        if (customer != null)
        {
            _customerList.Remove(customer);
            var result = _fileService.SaveToFile(JsonConvert.SerializeObject(_customerList, Formatting.Indented));
            return ServiceResponse.Success;
        }

        return ServiceResponse.Failed;
    }

}
