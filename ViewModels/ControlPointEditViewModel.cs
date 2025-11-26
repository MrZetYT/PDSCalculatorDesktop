using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;

namespace PDSCalculatorDesktop.ViewModels
{
    public class ControlPointEditViewModel : ViewModelBase
    {
        private readonly IControlPointService _controlPointService;
        private readonly IWaterUseTypeService _waterUseTypeService;
        private readonly ControlPoint? _originalControlPoint;
        private readonly Window _window;

        private string _number = string.Empty;
        private string _name = string.Empty;
        private WaterUseType? _selectedWaterUseType;
        private ObservableCollection<WaterUseType> _waterUseTypes = new();
        private bool _isSaving = false;

        public string Number
        {
            get => _number;
            set
            {
                if (SetProperty(ref _number, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public WaterUseType? SelectedWaterUseType
        {
            get => _selectedWaterUseType;
            set
            {
                if (SetProperty(ref _selectedWaterUseType, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public ObservableCollection<WaterUseType> WaterUseTypes
        {
            get => _waterUseTypes;
            set => SetProperty(ref _waterUseTypes, value);
        }

        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
        }

        public string WindowTitle => _originalControlPoint == null
            ? "Добавление контрольного створа"
            : "Редактирование контрольного створа";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ControlPointEditViewModel(
            IControlPointService controlPointService,
            IWaterUseTypeService waterUseTypeService,
            Window window,
            ControlPoint? controlPoint = null)
        {
            _controlPointService = controlPointService;
            _waterUseTypeService = waterUseTypeService;
            _window = window;
            _originalControlPoint = controlPoint;

            SaveCommand = new RelayCommand(
                execute: _ => SaveAsync(),
                canExecute: _ => CanSave()
            );

            CancelCommand = new RelayCommand(_ => Cancel());

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                var waterUseTypes = await _waterUseTypeService.GetAllWaterUseTypesAsync();
                WaterUseTypes.Clear();
                foreach (var type in waterUseTypes)
                {
                    WaterUseTypes.Add(type);
                }

                if (_originalControlPoint != null)
                {
                    Number = _originalControlPoint.Number;
                    Name = _originalControlPoint.Name;
                    SelectedWaterUseType = WaterUseTypes.FirstOrDefault(w => w.Id == _originalControlPoint.WaterUseTypeId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSave()
        {
            return !IsSaving
                && !string.IsNullOrWhiteSpace(Number)
                && !string.IsNullOrWhiteSpace(Name)
                && SelectedWaterUseType != null;
        }

        private async void SaveAsync()
        {
            IsSaving = true;
            try
            {
                if (_originalControlPoint == null)
                {
                    await _controlPointService.CreateControlPointAsync(
                        Number.Trim(),
                        Name.Trim(),
                        SelectedWaterUseType!.Id);

                    MessageBox.Show(
                        "Контрольный створ успешно добавлен!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    await _controlPointService.UpdateControlPointAsync(
                        _originalControlPoint.Id,
                        Number.Trim(),
                        Name.Trim(),
                        SelectedWaterUseType!.Id
                    );

                    MessageBox.Show(
                        "Контрольный створ успешно обновлен!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }

                _window.DialogResult = true;
                _window.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Произошла ошибка: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            finally
            {
                IsSaving = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}