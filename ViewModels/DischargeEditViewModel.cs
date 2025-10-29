using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PDSCalculatorDesktop.ViewModels
{
    public class DischargeEditViewModel : ViewModelBase
    {
        private readonly IDischargeService _dischargeService;
        private readonly Discharge? _originalDischarge;
        private readonly Window _window;

        private string _code = string.Empty;
        private string _name = string.Empty;
        private DateTime _registrationAt = DateTime.MinValue;
        private int _enterpriseId = 0;
        private int _controlPointId = 0;

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

        public DateTime RegistrationAt
        {
            get => _registrationAt;
            set
            {
                if (SetProperty(ref _registrationAt, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int EnterpriseId
        {
            get => _enterpriseId;
            set
            {
                if (SetProperty(ref _enterpriseId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public int ControlPointId
        {
            get => _controlPointId;
            set
            {
                if (SetProperty(ref _controlPointId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string WindowTitle => _originalDischarge == null
            ? "Добавление выпуска"
            : "Редактирование выпуска";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public DischargeEditViewModel(
            IDischargeService dischargeService,
            Window window,
            Discharge? discharge = null)
        {
            _dischargeService = dischargeService;
            _window = window;
            _originalDischarge = discharge;

            if (_originalDischarge != null)
            {
                Code = _originalDischarge.Code;
                Name = _originalDischarge.Name;
                RegistrationAt = _originalDischarge.RegistrationAt;
                EnterpriseId = _originalDischarge.EnterpriseId;
                ControlPointId = _originalDischarge.ControlPointId;
            }

            SaveCommand = new RelayCommand(
                execute: _ => SaveAsync(),
                canExecute: _ => CanSave()
            );

            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Code)
                && !string.IsNullOrWhiteSpace(Name)
                && !int.IsNegative(EnterpriseId)
                && !int.IsNegative(ControlPointId);
        }

        private async void SaveAsync()
        {
            try
            {
                if (_originalDischarge == null)
                {
                    await _dischargeService.CreateDischargeAsync(Code.Trim(), Name.Trim(), RegistrationAt, EnterpriseId, ControlPointId);

                    MessageBox.Show(
                        "Выпуск успешно добавлен!",
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
                        RegistrationAt,
                        EnterpriseId,
                        ControlPointId
                    );

                    MessageBox.Show(
                        "Выпуск успешно обновлен!",
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
        }
        private void Cancel()
        {
            _window.DialogResult = false;
            _window.Close();
        }
    }
}