using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;

namespace PDSCalculatorDesktop.ViewModels
{
    public class EnterpriseViewModel : ViewModelBase
    {

        private readonly IEnterpriseService _enterpriseService;
        private Enterprise? _selectedEnterprise;
        private string _searchText = string.Empty;

        public ObservableCollection<Enterprise> Enterprises { get; set; }

        public Enterprise? SelectedEnterprise
        {
            get => _selectedEnterprise;
            set
            {
                if (SetProperty(ref _selectedEnterprise, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    LoadEnterprisesAsync();
                }
            }
        }

        public ICommand AddCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand RefreshCommand { get; }

        public EnterpriseViewModel(IEnterpriseService enterpriseService)
        {
            _enterpriseService = enterpriseService;
            Enterprises = new ObservableCollection<Enterprise>();

            AddCommand = new RelayCommand(_ => AddEnterprise());

            EditCommand = new RelayCommand(
                execute: _ => EditEnterprise(),
                canExecute: _ => SelectedEnterprise != null
            );

            DeleteCommand = new RelayCommand(
                execute: _ => DeleteEnterpriseAsync(),
                canExecute: _ => SelectedEnterprise != null
            );

            RefreshCommand = new RelayCommand(_ => LoadEnterprisesAsync());

            LoadEnterprisesAsync();
        }

        private async void LoadEnterprisesAsync()
        {
            try
            {
                var enterprises = await _enterpriseService.GetAllEnterprisesAsync();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    enterprises = enterprises.Where(e =>
                        e.Code.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    );
                }

                Enterprises.Clear();
                foreach (var enterprise in enterprises)
                {
                    Enterprises.Add(enterprise);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddEnterprise()
        {
            // TODO: Открыть окно добавления предприятия
            MessageBox.Show("Функция добавления предприятия будет реализована далее",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditEnterprise()
        {
            if (SelectedEnterprise == null) return;

            // TODO: Открыть окно редактирования с данными SelectedEnterprise
            MessageBox.Show($"Редактирование предприятия: {SelectedEnterprise.Name}",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void DeleteEnterpriseAsync()
        {
            if (SelectedEnterprise == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить предприятие '{SelectedEnterprise.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes) return;

            try
            {
                await _enterpriseService.DeleteEnterpriseAsync(SelectedEnterprise.Id);

                Enterprises.Remove(SelectedEnterprise);

                MessageBox.Show("Предприятие успешно удалено", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}