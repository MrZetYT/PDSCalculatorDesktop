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
    public class SubstanceEditViewModel : ViewModelBase
    {
        private readonly ISubstanceService _substanceService;
        private readonly Substance? _originalSubstance;
        private readonly Window _window;

        private string _code = string.Empty;
        private string _name = string.Empty;
        private double _knk = 0;
        private bool _isSaving = false;
        private int _selectedWaterUseTypeId = 0;

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

        public double KNK
        {
            get => _knk;
            set
            {
                if (SetProperty(ref _knk, value))
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

        public int SelectedWaterUseTypeId
        {
            get => _selectedWaterUseTypeId;
            set
            {
                if (SetProperty(ref _selectedWaterUseTypeId, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string WindowTitle => _originalSubstance == null
            ? "Добавление загрязняющего вещества"
            : "Редактирование загрязняющего вещества";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public SubstanceEditViewModel(
            ISubstanceService substanceService,
            Window window,
            Substance? substance = null)
        {
            _substanceService = substanceService;
            _window = window;
            _originalSubstance = substance;

            if (_originalSubstance != null)
            {
                Code = _originalSubstance.Code;
                Name = _originalSubstance.Name;
                KNK = _originalSubstance.KNK;
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
                && !string.IsNullOrWhiteSpace(Name)
                && KNK >= 0;
        }

        private async void SaveAsync()
        {
            IsSaving = true;
            try
            {
                if (_originalSubstance == null)
                {
                    await _substanceService.CreateSubstanceAsync(
                        Code.Trim(),
                        Name.Trim(),
                        KNK);

                    MessageBox.Show(
                        "Загрязняющее вещество успешно добавлено!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    await _substanceService.UpdateSubstanceAsync(
                        _originalSubstance.Id,
                        Code.Trim(),
                        Name.Trim(),
                        KNK
                    );

                    MessageBox.Show(
                        "Загрязняющее вещество успешно обновлено!",
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