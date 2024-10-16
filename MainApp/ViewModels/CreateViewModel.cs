using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MainApp.Shared.Interfaces;
using MainApp.Shared.Models;
using Microsoft.Extensions.DependencyInjection;


namespace MainApp.ViewModels;

public partial class CreateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICustomerService _customerService;
    public List<Category> Categories { get; set; }


    public CreateViewModel(IServiceProvider serviceProvider, ICustomerService customerService)
    {
        _serviceProvider = serviceProvider;
        _customerService = customerService;

        Categories = new List<Category>
        {
            new Category { Id = 1, Name = "Boat" },
            new Category { Id = 2, Name = "Car" },
            new Category { Id = 3, Name = "Motorcycle" }
        };
}

    [ObservableProperty]
    private Customer customer = new();



    [RelayCommand]
    public void Save()
    {
        var result = _customerService.SaveCustomer(Customer);
        if (result == Shared.Enums.ServiceResponse.Success || result == Shared.Enums.ServiceResponse.Exists)
        {
            //Navigate back to Overview
            var viewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            viewModel.CurrentViewModel = _serviceProvider.GetRequiredService<OverviewViewModel>();
        }
    }
    [RelayCommand]
    public void Cancel()
    {
        var viewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        viewModel.CurrentViewModel = _serviceProvider.GetRequiredService<OverviewViewModel>();
    }
}
