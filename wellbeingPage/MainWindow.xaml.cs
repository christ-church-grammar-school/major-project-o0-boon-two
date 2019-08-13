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
using System.Windows.Threading;

namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer sec = new DispatcherTimer();
        DispatcherTimer milli = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Popup.Visibility = Visibility.Collapsed;

            sec.Tick += new EventHandler(oneSecond);
            sec.Interval = new TimeSpan(0, 0, 1);
            sec.Start();

            milli.Interval = TimeSpan.FromMilliseconds(1);
            milli.Tick += updateSecondHand;
            milli.Start();

        }
        void updateSecondHand(object sender, object e)
        {
            secondHand.Angle = (DateTime.Now.Second + (double)DateTime.Now.Millisecond / 1000)*6;
        }
        private void oneSecond(object sender, EventArgs e)
        {
            TimerTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
            minuteHand1.Angle = (DateTime.Now.Minute + (double)DateTime.Now.Second / 60) * 6;
            hourhand1.Angle = (DateTime.Now.Hour + (double)DateTime.Now.Minute / 60) * 15;
        }

        private void TasksClicked(object sender, RoutedEventArgs e)
        {
            ShowTasksPage();
        }

        private void WellbeingClicked(object sender, RoutedEventArgs e)
        {
            ShowWellbeingPage();
        }

        private void GymClicked(object sender, RoutedEventArgs e)
        {
            ShowGymPage();
        }

        private void LiveMarksClicked(object sender, RoutedEventArgs e)
        {
            ShowLiveMarksPage();
        }

        private void ShowTasksPage()
        {
            TasksPage page = new TasksPage();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }

        private void ShowWellbeingPage()
        {
            wellbeing page = new wellbeing();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }
        private void ShowGymPage()
        {
            Gym page = new Gym();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }
        private void ShowLiveMarksPage()
        {
            Marks page = new Marks();
            var contentCopy = Content;
            Content = page;
            page.ladder += (object sender, EventArgs e) =>
            {
                Content = contentCopy;
            };
        }

        private void DarknessButtonScreenClicked(object sender, RoutedEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
        }

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            Popup.Visibility = Visibility.Visible;
        }
    }
}

