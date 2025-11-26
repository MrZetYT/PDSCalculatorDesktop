using System;
using System.Windows;
using System.Windows.Input;
using PDSCalculatorDesktop.Commands;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Services;

namespace PDSCalculatorDesktop.ViewModels
{
    public class EnterpriseEditViewModel : ViewModelBase
    {
        private readonly IEnterpriseService _enterpriseService;
        private readonly Enterprise? _originalEnterprise;
        private readonly Window _window;

        private string _code = string.Empty;
        private string _name = string.Empty;
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

        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
        }

        public string WindowTitle => _originalEnterprise == null
            ? "Добавление предприятия"
            : "Редактирование предприятия";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EnterpriseEditViewModel(
            IEnterpriseService enterpriseService,
            Window window,
            Enterprise? enterprise = null)
        {
            _enterpriseService = enterpriseService;
            _window = window;
            _originalEnterprise = enterprise;

            if (_originalEnterprise != null)
            {
                Code = _originalEnterprise.Code;
                Name = _originalEnterprise.Name;
            }

            SaveCommand = new RelayCommand(
                execute: _ => SaveAsync(),
                canExecute: _ => CanSave()
            );

            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private bool CanSave()
        {
            return !IsSaving
                && !string.IsNullOrWhiteSpace(Code)
                && !string.IsNullOrWhiteSpace(Name);
        }

        private async void SaveAsync()
        {
            IsSaving = true;
            try
            {
                if (_originalEnterprise == null)
                {
                    await _enterpriseService.CreateEnterpriseAsync(Code.Trim(), Name.Trim());

                    MessageBox.Show(
                        "Предприятие успешно добавлено!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    await _enterpriseService.UpdateEnterpriseAsync(
                        _originalEnterprise.Id,
                        Code.Trim(),
                        Name.Trim()
                    );

                    MessageBox.Show(
                        "Предприятие успешно обновлено!",
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