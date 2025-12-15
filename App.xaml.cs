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
            services.AddScoped<IWaterUseTypeRepository, WaterUseTypeRepository>();
            services.AddScoped<IBackgroundConcentrationRepository, BackgroundConcentrationRepository>();
            services.AddScoped<IDischargeConcentrationRepository, DischargeConcentrationRepository>();

            // Регистрация Services
            services.AddScoped<IEnterpriseService, EnterpriseService>();
            services.AddScoped<IDischargeService, DischargeService>();
            services.AddScoped<IControlPointService, ControlPointService>();
            services.AddScoped<ISubstanceService, SubstanceService>();
            services.AddScoped<IWaterUseTypeService, WaterUseTypeService>();
            services.AddScoped<IBackgroundConcentrationService, BackgroundConcentrationService>();
            services.AddScoped<IDischargeConcentrationService, DischargeConcentrationService>();
            services.AddScoped<ICalculationDataProvider, CalculationDataProvider>();
            services.AddScoped<IPDSCalculationService, PDSCalculationService>();

            // Регистрация ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<EnterpriseViewModel>();
            services.AddTransient<DischargeViewModel>();
            services.AddTransient<ControlPointViewModel>();
            services.AddTransient<SubstanceViewModel>();
            services.AddTransient<PDSCalculationViewModel>();
            services.AddTransient<DischargeDetailsViewModel>();

            // Регистрация Views
            services.AddTransient<MainWindow>();
            services.AddTransient<EnterpriseView>();
            services.AddTransient<DischargeView>();
            services.AddTransient<ControlPointView>();
            services.AddTransient<SubstanceView>();
            services.AddTransient<PDSCalculationView>();
            services.AddTransient<DischargeDetailsView>();
            services.AddTransient<TechnicalParametersEditView>();
            services.AddTransient<ConcentrationEditView>();

            _serviceProvider = services.BuildServiceProvider();

            // Запускаем главное окно
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