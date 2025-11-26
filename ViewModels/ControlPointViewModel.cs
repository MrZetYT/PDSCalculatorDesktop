using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;
using PDSCalculatorDesktop.Views;

namespace PDSCalculatorDesktop.ViewModels
{
    public class ControlPointViewModel : ViewModelBase
    {
        private readonly IControlPointService _controlPointService;
        private readonly IWaterUseTypeService _waterUseTypeService;
        private ControlPoint? _selectedControlPoint;
        private string _searchText = string.Empty;
        private bool _isLoading = false;

        public ObservableCollection<ControlPoint> ControlPoints { get; }

        public ControlPoint? SelectedControlPoint
        {
            get => _selectedControlPoint;
            set
            {
                if (SetProperty(ref _selectedControlPoint, value))
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
                    LoadControlPointsAsync();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        public ControlPointViewModel(
            IControlPointService controlPointService,
            IWaterUseTypeService waterUseTypeService)
        {
            _controlPointService = controlPointService;
            _waterUseTypeService = waterUseTypeService;
            ControlPoints = new ObservableCollection<ControlPoint>();

            AddCommand = new RelayCommand(_ => AddControlPoint());

            EditCommand = new RelayCommand(
                execute: _ => EditControlPoint(),
                canExecute: _ => SelectedControlPoint != null
            );

            DeleteCommand = new RelayCommand(
                execute: _ => DeleteControlPointAsync(),
                canExecute: _ => SelectedControlPoint != null
            );

            RefreshCommand = new RelayCommand(_ => LoadControlPointsAsync());

            LoadControlPointsAsync();
        }

        private async void LoadControlPointsAsync()
        {
            IsLoading = true;
            try
            {
                var controlPoints = await _controlPointService.GetAllControlPointsAsync();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    controlPoints = controlPoints.Where(c =>
                        c.Number.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        (c.WaterUseType?.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                    );
                }

                ControlPoints.Clear();
                foreach (var controlPoint in controlPoints)
                {
                    ControlPoints.Add(controlPoint);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddControlPoint()
        {
            var window = new ControlPointEditView();
            var viewModel = new ControlPointEditViewModel(
                _controlPointService,
                _waterUseTypeService,
                window,
                null
            );

            window.DataContext = viewModel;

            if (window.ShowDialog() == true)
            {
                LoadControlPointsAsync();
            }
        }

        private void EditControlPoint()
        {
            if (SelectedControlPoint == null) return;

            var window = new ControlPointEditView();
            var viewModel = new ControlPointEditViewModel(
                _controlPointService,
                _waterUseTypeService,
                window,
                SelectedControlPoint
            );

            window.DataContext = viewModel;

            if (window.ShowDialog() == true)
            {
                LoadControlPointsAsync();
            }
        }

        private async void DeleteControlPointAsync()
        {
            if (SelectedControlPoint == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить контрольный створ '{SelectedControlPoint.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes) return;

            try
            {
                await _controlPointService.DeleteControlPointAsync(SelectedControlPoint.Id);
                ControlPoints.Remove(SelectedControlPoint);

                MessageBox.Show("Контрольный створ успешно удален", "Успех",
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