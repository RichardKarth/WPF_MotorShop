using MainApp.Shared.Interfaces;
using MainApp.Shared.Services;
using MainApp.ViewModels;
using MainApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace MainApp;


public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    public void ConfigureServices(IServiceCollection services)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Combine(baseDirectory, "customer.json");

        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddSingleton<IFileService>(new FileService(filePath));

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddTransient<OverviewViewModel>();
        services.AddTransient<OverviewView>();

        services.AddTransient<EditViewModel>();
        services.AddTransient<EditView>();

        services.AddTransient<CreateViewModel>();
        services.AddTransient<CreateView>();
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
        mainWindow.Show();

        base.OnStartup(e);
    }


}
