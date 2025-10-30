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
    public class ControlPointViewModel : ViewModelBase
    {
        private readonly IControlPointService _controlPointService;
        private ControlPoint? _selectedControlPoint;

        public ObservableCollection<ControlPoint> ControlPoints { get; set; }

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

        public ICommand AddCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand RefreshCommand { get; }

        public ControlPointViewModel(IControlPointService controlPointService)
        {
            _controlPointService = controlPointService;
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
            try
            {
                var controlPoints = await _controlPointService.GetAllControlPointsAsync();

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
        }

        private void AddControlPoint()
        {
            var window = new ControlPointEditView();

            var viewModel = new ControlPointEditViewModel(
                _controlPointService,
                window,
                null
            );

            window.DataContext = viewModel;

            var result = window.ShowDialog();

            if (result == true)
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
                window,
                SelectedControlPoint
            );

            window.DataContext = viewModel;

            var result = window.ShowDialog();

            if (result == true)
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

                MessageBox.Show("Створ успешно удален", "Успех",
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
