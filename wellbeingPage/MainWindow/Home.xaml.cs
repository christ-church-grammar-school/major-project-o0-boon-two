﻿using System;
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
using System.Windows.Threading;


namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        DispatcherTimer seconds = new DispatcherTimer();
        DispatcherTimer milliseconds = new DispatcherTimer();

        public Home()
        {
            InitializeComponent();
            
            App.Current.Properties["LoadThing"] = "1";

            secondHand.Angle = (DateTime.Now.Second + (double)DateTime.Now.Millisecond / 1000) * 6 + 90;
            minuteHand1.Angle = (DateTime.Now.Minute + (double)DateTime.Now.Second / 60) * 6 + 90;
            hourhand1.Angle = (DateTime.Now.Hour + (double)DateTime.Now.Minute / 60) * 30 + 90;

            seconds.Tick += new EventHandler(OneSecond);
            seconds.Interval = new TimeSpan(0, 0, 1);
            seconds.Start();

            milliseconds.Interval = TimeSpan.FromMilliseconds(1);
            milliseconds.Tick += UpdateSecondHand;
            milliseconds.Start();
        }

       

        void UpdateSecondHand(object sender, object e)
        {
            secondHand.Angle = (DateTime.Now.Second + (double)DateTime.Now.Millisecond / 1000) * 6 + 90;
        }
        private void OneSecond(object sender, EventArgs e)
        {
            minuteHand1.Angle = (DateTime.Now.Minute + (double)DateTime.Now.Second / 60) * 6 + 90;
            hourhand1.Angle = (DateTime.Now.Hour + (double)DateTime.Now.Minute / 60) * 30 + 90;
        }
        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var yratio = e.NewSize.Height / 764.5;
            var xratio = e.NewSize.Width / 1187;

            SecondHand.Margin = new Thickness(0, 0, xratio * 599, yratio * 384);
            MinuteHand.Margin = new Thickness(0, 0, xratio * 596, yratio * 382);
            HourHand.Margin = new Thickness(0, 0, xratio * 593, yratio * 379.5);
        }
        
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).SettingsPopup.Visibility = Visibility.Visible;
            
            ((MainWindow)System.Windows.Application.Current.MainWindow).UserField.Text = MainWindow.info.Username;
            
            ((MainWindow)System.Windows.Application.Current.MainWindow).PassField.Password = MainWindow.info.Password;
            ((MainWindow)System.Windows.Application.Current.MainWindow).UsernumField.Text= MainWindow.info.StudentNO;

        }

        private void changed(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
