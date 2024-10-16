using MainApp.Shared.Enums;
using MainApp.Shared.Models;

namespace MainApp.Shared.Interfaces
{
    public interface ICustomerService
    {
        ServiceResponse DeleteCustomer(string id);
        IEnumerable<Customer> GetCustomer();
        ServiceResponse SaveCustomer(Customer customer);
        ServiceResponse UpdateCustomer(Customer customer);
    }
}