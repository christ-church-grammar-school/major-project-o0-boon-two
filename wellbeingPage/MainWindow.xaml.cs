using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new Home();
        }
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var yratio = e.NewSize.Height / 764.5;
            var xratio = e.NewSize.Width / 1187;

            /*SecondHand.Margin = new Thickness(0, 0, xratio * 599, yratio * 384);
            MinuteHand.Margin = new Thickness(0, 0, xratio * 596, yratio * 382);
            HourHand.Margin = new Thickness(0, 0, xratio * 593, yratio * 379.5);*/
        }
        private void DarknessButtonScreenClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
        }

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Visible;
        }


        private void TasksClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new TasksPage();
        }

        private void WellbeingClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new wellbeing();
        }

        private void GymClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Gym();
        }

        private void LiveMarksClicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Marks();
        }
    }
}

