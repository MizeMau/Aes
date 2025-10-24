using Aes.Database.Table.Transaction;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aes.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }


        // ===== VIEWMODEL =====
        public class SettingsViewModel : INotifyPropertyChanged
        {
            private readonly Database.Table.Transaction.Category.Service _service;
            public Array CategoryTypes => Enum.GetValues(typeof(Category.Type));

            public ObservableCollection<Category.Model> Categories { get; set; }
            private Category.Model _selectedCategory;

            public Category.Model SelectedCategory
            {
                get => _selectedCategory;
                set
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsCategorySelected)); // Notify visibility binding
                    OnPropertyChanged(nameof(IsCategorySelectedNeg)); // Notify visibility binding
                    UpdateCommand?.RaiseCanExecuteChanged();
                    DeleteCommand?.RaiseCanExecuteChanged();
                }
            }

            public RelayCommand AddCommand { get; }
            public RelayCommand UpdateCommand { get; }
            public RelayCommand CancelCommand { get; }
            public RelayCommand DeleteCommand { get; }
            public bool IsCategorySelected => SelectedCategory?.TransactionCategoryID > 0;
            public bool IsCategorySelectedNeg => !IsCategorySelected;

            public SettingsViewModel()
            {
                _service = new Database.Table.Transaction.Category.Service();
                Categories = new ObservableCollection<Category.Model>(_service.GetAll());
                SelectedCategory = new Category.Model();

                AddCommand = new RelayCommand(AddCategory);
                UpdateCommand = new RelayCommand(UpdateCategory, () => SelectedCategory?.TransactionCategoryID > 0);
                CancelCommand = new RelayCommand(CancelCategory);
                DeleteCommand = new RelayCommand(DeleteCategory, () => SelectedCategory?.TransactionCategoryID > 0);
            }

            private void AddCategory()
            {
                try
                {
                    _service.Create(SelectedCategory);
                    Refresh();
                    SelectedCategory = new Category.Model();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding category:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void UpdateCategory()
            {
                try
                {
                    _service.Update(SelectedCategory);
                    Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating category:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void CancelCategory()
            {
                SelectedCategory = new Category.Model();
            }

            private void DeleteCategory()
            {
                try
                {
                    if (MessageBox.Show($"Delete category '{SelectedCategory.Name}'?", "Confirm Delete",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        _service.Delete(SelectedCategory.TransactionCategoryID);
                        Refresh();
                        SelectedCategory = new Category.Model();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting category:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private void Refresh()
            {
                Categories.Clear();
                foreach (var c in _service.GetAll())
                    Categories.Add(c);
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        // ===== RELAY COMMAND =====
        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool>? _canExecute;

            public RelayCommand(Action execute, Func<bool>? canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public event EventHandler? CanExecuteChanged;
            public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
            public void Execute(object? parameter) => _execute();
            public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
