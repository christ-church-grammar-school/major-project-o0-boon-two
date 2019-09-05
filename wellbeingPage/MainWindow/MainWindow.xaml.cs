using System;
using System.Collections.Generic;
using System.IO;
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
using wellbeingPage.Settings;

namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool ShutAll = true;  //when this window shuts everything shuts
        DispatcherTimer milliseconds = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new Home();

          
            if (!File.Exists("data/cred.txt")) // IF THIS PERSON HAS NOT USED THE APP BEFORE
            {

                Preferences SettingsWin = new Preferences();
                SettingsWin.Show();
                SettingsWin.SettingsFrame.Content = new Login();
                SettingsWin.LoginStuff.Visibility = Visibility.Visible;
                ShutAll = false;
                this.Close();
            } else
            {
                //GetStudentData.DownloadLiveMarks(Lines[0], Lines[1], true);

                if (File.Exists("data/marks/Subject0.txt")){ // if marks have been downloaded: parse marks 
                    GetStudentData.PutMarks();
                }
            }

            milliseconds.Interval = TimeSpan.FromMilliseconds(1);
            milliseconds.Tick += Rot;
            milliseconds.Start();
        }
        void Rot(object sender, object e)
        {
            if (!ReloadButton.IsEnabled)
            {
                ReloadRotater.Angle += 3;
            }
            
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
            MenuPopup.Visibility = Visibility.Collapsed;
            ShowAllPages();
            TasksSection.Visibility = Visibility.Collapsed;
            MainFrame.Content = new TasksPage();
        }

        private void WellbeingClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
            ShowAllPages();
            WellbeingSection.Visibility = Visibility.Collapsed;
            MainFrame.Content = new wellbeing();
        }

        private void GymClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
            ShowAllPages();
            GymSection.Visibility = Visibility.Collapsed;
            MainFrame.Content = new Gym();
        }

        private void LiveMarksClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
            ShowAllPages();
            MarksSection.Visibility = Visibility.Collapsed;
            LastUp.Visibility = Visibility.Visible;
            ReloadButton.Visibility = Visibility.Visible;
            MainFrame.Content = new Marks();
        }
        private void HomeClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
            ShowAllPages();
            HomeSection.Visibility = Visibility.Collapsed;
            MainFrame.Content = new Home();
        }
        void ShowAllPages()
        {
            LastUp.Visibility = Visibility.Collapsed;
            ReloadButton.Visibility = Visibility.Collapsed;
            ReloadRotater.Angle = 0;
            HomeSection.Visibility = Visibility.Visible;
            TasksSection.Visibility = Visibility.Visible;
            WellbeingSection.Visibility = Visibility.Visible;
            GymSection.Visibility = Visibility.Visible;
            MarksSection.Visibility = Visibility.Visible;
        }

        private void MainWinClosed(object sender, EventArgs e)
        {
            if (ShutAll)
            {
                Application.Current.Shutdown();
            }
        }
        private void ReloadMarks(object sender, RoutedEventArgs e)
        {
            GetStudentData.DownloadLiveMarks(true);
            ReloadButton.IsEnabled = false;
        }

    }
}

