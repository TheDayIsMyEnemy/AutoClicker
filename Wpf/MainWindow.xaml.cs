using Core;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf
{
    public partial class MainWindow : Window
    {
        private readonly Clicker clicker = new Clicker();
        public MainWindow()
        {
            InitializeComponent();
            Stop.IsEnabled = false;
            Title = "Noob Click 2019";
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!clicker.IsRunning && Start.IsEnabled)
            {
                Start.IsEnabled = false;
                Stop.IsEnabled = true;

                clicker.Reset();

                int x, y, hrs, mins, secs, ms, startDelay;
                ParseInput(out x, out y, out hrs, out mins, out secs, out ms, out startDelay);

                clicker.X = x;
                clicker.Y = y;
                clicker.Hours = hrs;
                clicker.Minutes = mins;
                clicker.Seconds = secs;
                clicker.Ms = ms;
                clicker.Click = (ClickType)comboBoxClicks.SelectedValue;
                clicker.Button = (ButtonType)comboBoxButtons.SelectedValue;

                if (SendText.IsChecked == true)
                {
                    clicker.Message = InputText.Text;
                }

                await clicker.Start(startDelay);
            }
        }

        private void ParseInput(out int x, out int y, out int hrs, out int mins, out int secs, out int ms, out int startDelay)
        {
            int.TryParse(X.Text, out x);
            int.TryParse(Y.Text, out y);
            int.TryParse(Hr.Text, out hrs);
            int.TryParse(Mins.Text, out mins);
            int.TryParse(Secs.Text, out secs);
            int.TryParse(Ms.Text, out ms);
            int.TryParse(StartDelay.Text, out startDelay);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            clicker.Stop();
            Start.IsEnabled = true;
            Stop.IsEnabled = false;
        }

        private void ComboBoxButtons_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxButtons.ItemsSource = Enum.GetValues(typeof(ButtonType)).Cast<ButtonType>();
            comboBoxButtons.SelectedIndex = 0;
        }

        private void ComboBoxClicks_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxClicks.ItemsSource = Enum.GetValues(typeof(ClickType)).Cast<ClickType>();
            comboBoxClicks.SelectedIndex = 0;
        }
    }
}
