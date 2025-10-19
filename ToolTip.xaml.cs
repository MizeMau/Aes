using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Aes
{
    public partial class ToolTip : UserControl, IChartTooltip
    {
        public ToolTip()
        {
            InitializeComponent();
            DataContext = this;
        }

        public TooltipData Data { get; set; }

        private string _value { get; set; }
        public string Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }

        private Brush _fill { get; set; }
        public Brush Fill
        {
            get => _fill;
            set { _fill = value; OnPropertyChanged(nameof(Fill)); }
        }

        public TooltipSelectionMode? SelectionMode { get; set; }

        public void Show() => Visibility = Visibility.Visible;

        public void Hide() => Visibility = Visibility.Hidden;

        public void Move(Point point)
        {
            Canvas.SetLeft(this, point.X);
            Canvas.SetTop(this, point.Y);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
