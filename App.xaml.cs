using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Services;
using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Repositories.Interfaces;
using PDSCalculatorDesktop.Repositories;
using PDSCalculatorDesktop.ViewModels;
using PDSCalculatorDesktop.Views;

namespace PDSCalculatorDesktop
{
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddUserSecrets<App>()
                .Build();

            var services = new ServiceCollection();

            // Регистрация DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Регистрация Repository
            services.AddScoped<IEnterpriseRepository, EnterpriseRepository>();
            services.AddScoped<IDischargeRepository, DischargeRepository>();
            services.AddScoped<IControlPointRepository, ControlPointRepository>();
            services.AddScoped<ITechnicalParametersRepository, TechnicalParametersRepository>();

            // Регистрация Services
            services.AddScoped<IEnterpriseService, EnterpriseService>();
            services.AddScoped<IDischargeService, DischargeService>();

            // Регистрация ViewModels
            services.AddTransient<EnterpriseViewModel>();
            services.AddTransient<DischargeViewModel>();

            // Регистрация Views
            services.AddTransient<EnterpriseView>();
            services.AddTransient<DischargeView>();

            _serviceProvider = services.BuildServiceProvider();

            // Запускаем окно управления предприятиями вместо MainWindow
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}