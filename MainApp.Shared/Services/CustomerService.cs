
using MainApp.Shared.Enums;
using MainApp.Shared.Interfaces;
using MainApp.Shared.Models;
using Newtonsoft.Json;

namespace MainApp.Shared.Services;

public class CustomerService : ICustomerService
{
    
    private List<Customer> _customerList;
    private readonly IFileService _fileService;
    
    public CustomerService(IFileService fileService, List<Customer> customerList = null)
    {
        _fileService = fileService;
        _customerList = customerList ?? new();
    }
    public ServiceResponse SaveCustomer(Customer customer)
    {
        try
        {
            var existingEmail = _customerList.FirstOrDefault(x => x.Email == customer.Email);
            if (existingEmail is not null)
            {
                return ServiceResponse.Exists;
            }

            _customerList.Add(customer);
            var result = _fileService.SaveToFile(JsonConvert.SerializeObject(_customerList, Formatting.Indented));
            return ServiceResponse.Success;
        }
        catch
        {
            return ServiceResponse.Error;
        }
       
    }

    public ServiceResponse UpdateCustomer(Customer customer)
    {
        var existingUser = _customerList.FirstOrDefault(c => c.Id == customer.Id);
        try
        {
            if (existingUser is not null)
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
        try
        {
            var listContent = _fileService.GetFromFile();
            var tempList = JsonConvert.DeserializeObject<List<Customer>>(listContent);
            if (tempList != null && tempList.Count > 0)
            {
                _customerList = tempList;
            }
            return _customerList;
        }
        catch
        {
            return Enumerable.Empty<Customer>();
        }
       
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
