namespace DiConfigurationTest
{
    using System;
    using System.IO;
    using System.Windows;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.user.json", true, false);
            _configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            string userSetingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.user.json");

            services.ConfigureWritable<WindowSettings>(_configuration.GetSection("MainWindowSettings"), userSetingsPath);

            services.AddSingleton<MainWindow>();
        }
    }
}
