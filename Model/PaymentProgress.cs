using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aes.Model
{
    public class PaymentProgress : INotifyPropertyChanged
    {
        private decimal _progress;
        private decimal _max;
        private string _title;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public decimal Progress
        {
            get => _progress;
            set { _progress = value; OnPropertyChanged(nameof(Progress)); OnPropertyChanged(nameof(AmountText)); }
        }

        public decimal Max
        {
            get => _max;
            set { _max = value; OnPropertyChanged(nameof(Max)); OnPropertyChanged(nameof(AmountText)); }
        }

        public string AmountText => $"€{Progress}/€{Max}";

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
