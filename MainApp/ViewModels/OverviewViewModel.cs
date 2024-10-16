using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainApp.Shared.Interfaces;
using MainApp.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace MainApp.ViewModels
{
    public partial class OverviewViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICustomerService _customerService;

        public OverviewViewModel(IServiceProvider serviceProvider, ICustomerService customerService)
        {
            _serviceProvider = serviceProvider;
            _customerService = customerService;
            UpdateCustomerList();
        }
        [ObservableProperty]
        private ObservableCollection<Customer> customerList = [];
        private ObservableCollection<Category> categories = [];

        [RelayCommand]

        public void Add()
        {
            var viewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            viewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateViewModel>();
        }
        [RelayCommand]
        public void Edit(Customer customer)
        {
            var editViewModel = _serviceProvider.GetRequiredService<EditViewModel>();
            editViewModel.Customer = customer;

            var viewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            viewModel.CurrentViewModel = editViewModel;
        }
        [RelayCommand]
        public void Delete(string id)
        {
            _customerService.DeleteCustomer(id);
            UpdateCustomerList();
        }
        public void UpdateCustomerList()
        {
            CustomerList.Clear();
            foreach (var customer in _customerService.GetCustomer())
            {

                CustomerList.Add(customer);
            }
        }
    }
}
