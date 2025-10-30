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
using System.Collections.ObjectModel;

namespace PDSCalculatorDesktop.ViewModels
{
    public class SubstanceEditViewModel : ViewModelBase
    {
        private readonly ISubstanceService _substanceService;
        private readonly Substance? _originalSubstance;
        private readonly Window _window;

        public ObservableCollection<HazardClass> HazardClasses;

        private string _code = string.Empty;
        private string _name = string.Empty;
        private string _groupLFV = string.Empty;
        private HazardClass _hazardClass;

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

        public string GroupLFV
        {
            get => _groupLFV;
            set
            {
                if(SetProperty(ref _groupLFV, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public HazardClass HazardClass
        {
            get => _hazardClass;
            set
            {
                if(SetProperty(ref _hazardClass, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string WindowTitle => _originalSubstance == null
            ? "Добавление вещества"
            : "Редактирование вещества";

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

            HazardClasses = new ObservableCollection<HazardClass>(
                Enum.GetValues(typeof(HazardClass)).Cast<HazardClass>()
            );

            HazardClass = HazardClass.ModeratelyHazardous;

            if (_originalSubstance != null)
            {
                Code = _originalSubstance.Code;
                Name = _originalSubstance.Name;
                GroupLFV = _originalSubstance.GroupLFV;
                HazardClass = _originalSubstance.HazardClass;
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
                && !string.IsNullOrWhiteSpace(GroupLFV);
        }

        private async void SaveAsync()
        {
            try
            {
                if (_originalSubstance == null)
                {
                    await _substanceService.CreateSubstanceAsync(Code.Trim(), Name.Trim(), GroupLFV.Trim(), HazardClass);

                    MessageBox.Show(
                        "Вещество успешно добавлено!",
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
                        GroupLFV.Trim(),
                        HazardClass
                    );

                    MessageBox.Show(
                        "Вещество успешно обновлено!",
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
