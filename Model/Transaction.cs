using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aes.Model
{
    public class Transaction : INotifyPropertyChanged
    {
        private string _note;
        private DateOnly _date;
        private string _categoryName;
        private decimal _amount;
        private Database.Table.Transaction.Category.Type _categoryType;

        public string Note
        {
            get => _note;
            set { _note = value; OnPropertyChanged(nameof(Note)); }
        }
        public DateOnly Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(nameof(Date)); OnPropertyChanged(nameof(DateText)); }
        }
        public string CategoryName
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged(nameof(CategoryName)); }
        }
        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(nameof(Amount)); OnPropertyChanged(nameof(AmountText)); }
        }
        public Database.Table.Transaction.Category.Type CategoryType
        {
            get => _categoryType;
            set { _categoryType = value; OnPropertyChanged(nameof(CategoryType)); OnPropertyChanged(nameof(AmountText)); OnPropertyChanged(nameof(Color)); }
        }
        public string DateText => Date.ToShortDateString();
        public string AmountText => (_categoryType == Database.Table.Transaction.Category.Type.Income ? "" : "-") + $"€{Amount}";
        public string Color => _categoryType == Database.Table.Transaction.Category.Type.Income ? "#4CAF50" : "#AF4C50";

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
