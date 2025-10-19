using LiveCharts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System.Text;
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

namespace Aes
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
            this.pieChart.DataHover += PieChart_DataHover;

            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += UpdateProgress;
            _timer.Start();
        }
        private void PieChart_DataHover(object sender, ChartPoint chartPoint)
        {
            var pieSeries = chartPoint.SeriesView as PieSeries;
            this.myCustomTooltip.Value = $"{chartPoint.Y}€ ({chartPoint.Participation:P0})";
            this.myCustomTooltip.Fill = pieSeries!.Fill;
        }
        private void UpdateProgress(object? sender, EventArgs e)
        {
            progressBar.Value += 2;
            if (progressBar.Value >= progressBar.Maximum)
                progressBar.Value = progressBar.Minimum;
        }
    }

    public class ViewModel
    {
        public SeriesCollection SeriesCollection { get; set; }


        public ViewModel()
        {
            SeriesCollection = new SeriesCollection
            {
                new PieSeries { Title = "Mary",  Values = new ChartValues<double> { 10 }, Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFF")!},
                new PieSeries { Title = "John",  Values = new ChartValues<double> { 20 }},
                new PieSeries { Title = "Alice", Values = new ChartValues<double> { 30 }},
                new PieSeries { Title = "Bob",   Values = new ChartValues<double> { 40 }},
                new PieSeries { Title = "Charlie", Values = new ChartValues<double> { 50 }},
            };
        }
    }
}
