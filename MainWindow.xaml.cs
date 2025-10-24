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
using System.Windows.Media.Animation;
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
            MainFrame.Navigate(new Pages.Home());
        }

        private bool _isMenuOpen = false;

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            double from = _isMenuOpen ? 0 : -220;
            double to = _isMenuOpen ? -220 : 0;

            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            SideMenuTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animation);
            _isMenuOpen = !_isMenuOpen;
        }

        private void HomePageButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.Home());
            CloseMenu();
        }

        private void SettingsPageButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.Settings());
            CloseMenu();
        }

        private void CloseMenu()
        {
            if (_isMenuOpen)
            {
                SettingsButton_Click(null, null);
            }
        }
    }
}
