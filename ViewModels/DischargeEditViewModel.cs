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
    public class DischargeEditViewModel : ViewModelBase
    {
        private readonly IDischargeService _dischargeService;
        private readonly IEnterpriseService _enterpriseService;
        private readonly IControlPointService _controlPointService;
        private readonly Discharge? _originalDischarge;
        private readonly Window _window;

        private string _code = string.Empty;
        private string _name = string.Empty;
        private DateTime _registrationDate = DateTime.Today;
        private Enterprise? _selectedEnterprise;
        private ControlPoint? _selectedControlPoint;
        private ObservableCollection<Enterprise> _enterprises = new();
        private ObservableCollection<ControlPoint> _controlPoints = new();
        private bool _isSaving = false;

        public string Code
        {
            get => _code;
            set
            {
                if (SetProperty(ref _code, value))
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

        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set
            {
                if (SetProperty(ref _registrationDate, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

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

        public ObservableCollection<Enterprise> Enterprises
        {
            get => _enterprises;
            set => SetProperty(ref _enterprises, value);
        }

        public ObservableCollection<ControlPoint> ControlPoints
        {
            get => _controlPoints;
            set => SetProperty(ref _controlPoints, value);
        }

        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
        }

        public string WindowTitle => _originalDischarge == null
            ? "Добавление выпуска сточных вод"
            : "Редактирование выпуска сточных вод";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public DischargeEditViewModel(
            IDischargeService dischargeService,
            IEnterpriseService enterpriseService,
            IControlPointService controlPointService,
            Window window,
            Discharge? discharge = null)
        {
            _dischargeService = dischargeService;
            _enterpriseService = enterpriseService;
            _controlPointService = controlPointService;
            _window = window;
            _originalDischarge = discharge;

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
                var enterprises = await _enterpriseService.GetAllEnterprisesAsync();
                var controlPoints = await _controlPointService.GetAllControlPointsAsync();

                Enterprises.Clear();
                foreach (var enterprise in enterprises)
                {
                    Enterprises.Add(enterprise);
                }

                ControlPoints.Clear();
                foreach (var controlPoint in controlPoints)
                {
                    ControlPoints.Add(controlPoint);
                }

                if (_originalDischarge != null)
                {
                    Code = _originalDischarge.Code;
                    Name = _originalDischarge.Name;
                    RegistrationDate = _originalDischarge.RegistrationDate.Date;
                    SelectedEnterprise = Enterprises.FirstOrDefault(e => e.Id == _originalDischarge.EnterpriseId);
                    SelectedControlPoint = ControlPoints.FirstOrDefault(c => c.Id == _originalDischarge.ControlPointId);
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
                && !string.IsNullOrWhiteSpace(Code)
                && !string.IsNullOrWhiteSpace(Name)
                && SelectedEnterprise != null
                && SelectedControlPoint != null;
        }

        private async void SaveAsync()
        {
            IsSaving = true;
            try
            {
                var registrationDateUtc = DateTime.SpecifyKind(RegistrationDate, DateTimeKind.Utc);

                if (_originalDischarge == null)
                {
                    await _dischargeService.CreateDischargeAsync(
                        Code.Trim(),
                        Name.Trim(),
                        registrationDateUtc,
                        SelectedEnterprise!.Id,
                        SelectedControlPoint!.Id
                    );

                    MessageBox.Show(
                        "Выпуск сточных вод успешно добавлен!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    await _dischargeService.UpdateDischargeAsync(
                        _originalDischarge.Id,
                        Code.Trim(),
                        Name.Trim(),
                        registrationDateUtc,
                        SelectedEnterprise!.Id,
                        SelectedControlPoint!.Id
                    );

                    MessageBox.Show(
                        "Выпуск сточных вод успешно обновлен!",
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