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
        private Database.Table.Transaction.Category.Model _category;
        private decimal _amount;
        private decimal _color;

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
        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(nameof(Amount)); OnPropertyChanged(nameof(AmountText)); }
        }
        public Database.Table.Transaction.Category.Model Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(nameof(Category)); OnPropertyChanged(nameof(AmountText)); OnPropertyChanged(nameof(Color)); }
        }
        public string DateText => Date.ToShortDateString();
        public string AmountText => (_category.Type == Database.Table.Transaction.Category.Type.Income ? "" : "-") + $"€{Amount}";
        public string Color => _category.Type == Database.Table.Transaction.Category.Type.Income ? "#4CAF50" : "#AF4C50";

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
