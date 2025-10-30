using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;
using PDSCalculatorDesktop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PDSCalculatorDesktop.ViewModels
{
    public class SubstanceViewModel : ViewModelBase
    {
        private readonly ISubstanceService _substanceService;
        private Substance? _selectedSubstance;
        private string _searchText = string.Empty;

        public ObservableCollection<Substance> Substances { get; set; }

        public Substance? SelectedSubstance
        {
            get => _selectedSubstance;
            set
            {
                if (SetProperty(ref _selectedSubstance, value))
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
                    LoadSubstancesAsync();
                }
            }
        }

        public ICommand AddCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand RefreshCommand { get; }

        public SubstanceViewModel(ISubstanceService substanceService)
        {
            _substanceService = substanceService;
            Substances = new ObservableCollection<Substance>();

            AddCommand = new RelayCommand(_ => AddSubstance());

            EditCommand = new RelayCommand(
                execute: _ => EditSubstance(),
                canExecute: _ => SelectedSubstance != null
            );

            DeleteCommand = new RelayCommand(
                execute: _ => DeleteSubstanceAsync(),
                canExecute: _ => SelectedSubstance != null
            );

            RefreshCommand = new RelayCommand(_ => LoadSubstancesAsync());

            LoadSubstancesAsync();
        }

        private async void LoadSubstancesAsync()
        {
            try
            {
                var substances = await _substanceService.GetAllSubstancesAsync();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    substances = substances.Where(e =>
                        e.Code.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        e.GroupLFV.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        e.HazardClass.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    );
                }

                Substances.Clear();
                foreach (var substance in substances)
                {
                    Substances.Add(substance);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddSubstance()
        {
            var window = new SubstanceEditView();

            var viewModel = new SubstanceEditViewModel(
                _substanceService,
                window,
                null
            );

            window.DataContext = viewModel;

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadSubstancesAsync();
            }
        }

        private void EditSubstance()
        {
            if (SelectedSubstance == null) return;

            var window = new SubstanceEditView();

            var viewModel = new SubstanceEditViewModel(
                _substanceService,
                window,
                SelectedSubstance
            );

            window.DataContext = viewModel;

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadSubstancesAsync();
            }
        }

        private async void DeleteSubstanceAsync()
        {
            if (SelectedSubstance == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить вещество '{SelectedSubstance.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes) return;

            try
            {
                await _substanceService.DeleteSubstanceAsync(SelectedSubstance.Id);

                Substances.Remove(SelectedSubstance);

                MessageBox.Show("Вещество успешно удалено", "Успех",
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
