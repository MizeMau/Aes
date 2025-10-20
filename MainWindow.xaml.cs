using Aes.Model;
using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Aes
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.pieChart.DataHover += PieChart_DataHover;

#if DEBUG
            var transactionCategoryService = new Database.Table.Transaction.Category.Service();
            var transactionRecurringService = new Database.Table.Transaction.Recurring.Service();
            var transactionTransactionService = new Database.Table.Transaction.Transaction.Service();

            if (!transactionCategoryService.GetQuery().Any())
            {
                var categories = new List<Database.Table.Transaction.Category.Model>
                {
                    new Database.Table.Transaction.Category.Model { Name = "Food", PlannedAmount = 100, Type = Database.Table.Transaction.Category.Type.Expense, Color = "#FF5733" },
                    new Database.Table.Transaction.Category.Model { Name = "Salary", Type = Database.Table.Transaction.Category.Type.Income, Color = "#33FF57" },
                    new Database.Table.Transaction.Category.Model { Name = "Entertainment", PlannedAmount = 40, Type = Database.Table.Transaction.Category.Type.Expense, Color = "#3357FF" },
                    new Database.Table.Transaction.Category.Model { Name = "Rent", PlannedAmount = 650, Type = Database.Table.Transaction.Category.Type.Expense, Color = "#7c5cff" },
                };
                transactionCategoryService.CreateRange(categories);
            }

            if (!transactionTransactionService.GetQuery().Any())
            {
                var transactions = new List<Database.Table.Transaction.Transaction.Model>
                {
                    new Database.Table.Transaction.Transaction.Model { Amount = 1000, Note = "Monthly Salary", Date = DateOnly.FromDateTime(DateTime.Now), TransactionCategoryID = 2 },
                    new Database.Table.Transaction.Transaction.Model { Amount = 50, Note = "Groceries", Date = DateOnly.FromDateTime(DateTime.Now), TransactionCategoryID = 1 },
                    new Database.Table.Transaction.Transaction.Model { Amount = 20, Note = "Movie Ticket", Date = DateOnly.FromDateTime(DateTime.Now), TransactionCategoryID = 3 },
                    new Database.Table.Transaction.Transaction.Model { Amount = 600, Note = "Movie Ticket", Date = DateOnly.FromDateTime(DateTime.Now), TransactionCategoryID = 4 },
                };
                transactionTransactionService.CreateRange(transactions);
            }
            //var recurrings = new List<Database.Table.Transaction.Recurring.Model>
            //{
            //    new Database.Table.Transaction.Recurring.Model { StartDate = DateOnly.FromDateTime(DateTime.Now), FrequencyType = Database.Table.Transaction.Recurring.FrequencyType.Monthly, FrequencyInterval = 1 },
            //    new Database.Table.Transaction.Recurring.Model { StartDate = DateOnly.FromDateTime(DateTime.Now), FrequencyType = Database.Table.Transaction.Recurring.FrequencyType.Weekly, FrequencyInterval = 1 }
            //};
#endif
            DataContext = new ViewModel();
        }
        private void PieChart_DataHover(object sender, ChartPoint chartPoint)
        {
            var pieSeries = chartPoint.SeriesView as PieSeries;
            this.myCustomTooltip.Value = $"{chartPoint.Y}€ ({chartPoint.Participation:P0})";
            this.myCustomTooltip.Fill = pieSeries!.Fill;
        }
        private void test(object? sender, EventArgs e)
        {
            if (!(DataContext is ViewModel))
                return;

            var transactionCategoryService = new Database.Table.Transaction.Category.Service();
            var test = transactionCategoryService.GetAll();

            var vm = (ViewModel)DataContext;
            var firstPayment = vm.Payments[0];
            firstPayment.Progress += 10;
        }
        private void DeleteTransaction(object? sender, EventArgs e)
        {
            if (!(DataContext is ViewModel))
                return;

            var vm = (ViewModel)DataContext;
            var firstPayment = vm.Payments[0];
            firstPayment.Progress += 10;
        }
        private void MoneyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only allow digits and comma/period
            e.Handled = !Regex.IsMatch(e.Text, @"[\d,\.]");
        }

        private void MoneyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox == null) return;

            string valueString = textBox.Text.Replace(" €", "").Trim();
            valueString = Regex.Replace(valueString, @"[^\d,\.]", "");

            decimal value = 0;
            var culture = CultureInfo.CreateSpecificCulture("de-DE");
            // Try to parse the numeric part
            if (decimal.TryParse(valueString, NumberStyles.Number, culture, out value))
            {
                string comma = "";

                // Check for trailing comma or period
                if (!string.IsNullOrEmpty(valueString) &&
                    (valueString[^1] == ',' || valueString[^1] == '.'))
                {
                    comma = ",";
                }

                textBox.Text = $"{value.ToString(culture)}{comma} €";
            }
            else
            {
                textBox.Text = "0 €";
            }

            // Move the caret just before the € symbol
            textBox.CaretIndex = textBox.Text.Length - 2;
        }
    }

    public class ViewModel
    {
        public SeriesCollection SeriesCollection { get; set; }
        public ObservableCollection<PaymentProgress> Payments { get; set; }
        public ObservableCollection<Transaction> Transactions { get; set; }
        public List<Database.Table.Transaction.Category.Model> Categories {get; set; }
        private Database.Table.Transaction.Category.Model _selectedCategory;
        public Database.Table.Transaction.Category.Model SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); }
        }
        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; OnPropertyChanged(nameof(SelectedDate)); }
        }

        private decimal _expense;
        public decimal Expense
        {
            get => _expense;
            set { _expense = value; OnPropertyChanged(nameof(Expense)); OnPropertyChanged(nameof(ExpenseText)); OnPropertyChanged(nameof(RemainderText)); }
        }
        private decimal _income;
        public decimal Income
        {
            get => _income;
            set { _income = value; OnPropertyChanged(nameof(Income)); OnPropertyChanged(nameof(IncomeText)); OnPropertyChanged(nameof(RemainderText)); }
        }
        public string IncomeText => $"€{Income}";
        public string ExpenseText => $"€{Expense}";
        public string RemainderText => $"€{Income - Expense}";
        public ViewModel()
        {
            var transactionCategoryService = new Database.Table.Transaction.Category.Service();
            var transactionTransactionService = new Database.Table.Transaction.Transaction.Service();
            Categories = transactionCategoryService.GetAll();
            SelectedCategory = Categories[0];
            SelectedDate = DateTime.Now;

            var transactions = transactionTransactionService.GetAllByMonth()
                .OrderByDescending(o => (o.Date, o.TransactionTransactionID))
                .ToList();

            var transactionsDict = transactions
                .GroupBy(g => g.TransactionCategoryID)
                .Select(s => new
                {
                    CategoryID = s.Key,
                    TotalAmount = s.Sum(sum => sum.Amount)
                })
                .ToDictionary(d => d.CategoryID, d => d.TotalAmount);
            decimal income = 0;
            decimal expense = 0;
            foreach (var dict in transactionsDict)
            {
                var category = Categories.Single(s => s.TransactionCategoryID == dict.Key);
                switch (category.Type)
                {
                    case Database.Table.Transaction.Category.Type.Income:
                        income += dict.Value;
                        break;
                    case Database.Table.Transaction.Category.Type.Expense:
                        expense += dict.Value;
                        break;
                }
            }
            Expense = expense;
            Income = income;
            SetOverviewData(transactions);
        }
        private void SetOverviewData(List<Database.Table.Transaction.Transaction.Model> transactions)
        {
            var transactionsDict = transactions
                .GroupBy(g => g.TransactionCategoryID)
                .Select(s => new
                {
                    CategoryID = s.Key,
                    TotalAmount = s.Sum(sum => sum.Amount)
                })
                .ToDictionary(d => d.CategoryID, d => d.TotalAmount);
            var pieSeries = Categories.Where(w => w.Type == Database.Table.Transaction.Category.Type.Expense)
                .Select(s => {
                    return new PieSeries
                    {
                        Title = s.Name,
                        Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(s.Color)!,
                        Values = new ChartValues<decimal> { transactionsDict[s.TransactionCategoryID] }
                    };
                });
            SeriesCollection = [
                .. pieSeries,
            ];

            var payments = new ObservableCollection<PaymentProgress>();
            Categories.Where(w => w.Type == Database.Table.Transaction.Category.Type.Expense)
                .ToList()
                .ForEach(f =>
                {
                    payments.Add(new PaymentProgress
                    {
                        Title = f.Name,
                        Max = f.PlannedAmount,
                        Progress = transactionsDict[f.TransactionCategoryID]
                    });
                });
            Payments = payments;

            var transactionsCollection = new ObservableCollection<Transaction>();
            foreach (var transcation in transactions)
            {
                var category = Categories.Single(s => s.TransactionCategoryID == transcation.TransactionCategoryID);
                transactionsCollection.Add(new Transaction()
                {
                    Note = transcation.Note,
                    Date = transcation.Date,
                    Category = category,
                    Amount = transcation.Amount,
                });
            }
            Transactions = transactionsCollection;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
