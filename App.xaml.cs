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
            services.AddScoped<ISubstanceRepository, SubstanceRepository>();

            // Регистрация Services
            services.AddScoped<IEnterpriseService, EnterpriseService>();
            services.AddScoped<IDischargeService, DischargeService>();
            services.AddScoped<IControlPointService, ControlPointService>();
            services.AddScoped<ISubstanceService,  SubstanceService>();

            // Регистрация ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<EnterpriseViewModel>();
            services.AddTransient<DischargeViewModel>();
            services.AddTransient<ControlPointViewModel>();
            services.AddTransient<SubstanceViewModel>();

            // Регистрация Views
            services.AddTransient<MainWindow>();
            services.AddTransient<EnterpriseView>();
            services.AddTransient<DischargeView>();
            services.AddTransient<ControlPointView>();
            services.AddTransient<SubstanceView>();

            _serviceProvider = services.BuildServiceProvider();

            // Запускаем окно управления предприятиями вместо MainWindow
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}