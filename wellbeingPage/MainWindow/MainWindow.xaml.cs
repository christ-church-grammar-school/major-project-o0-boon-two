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
using wellbeingPage.Settings;

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

          
            if (!File.Exists("data/cred.txt")) // IF THIS PERSON HAS NOT USED THE APP BEFORE
            {

                Preferences SettingsWin = new Preferences();
                SettingsWin.Show();
                SettingsWin.MainFrame.Content = new Login();
                SettingsWin.LoginStuff.Visibility = Visibility.Visible;
                this.Close();
            } else
            {
                List<string> Lines = new List<string>(System.IO.File.ReadAllLines("data/cred.txt"));



                //GetStudentData.DownloadLiveMarks(Lines[0], Lines[1], true);

                if (File.Exists("data/marks/Subject0.txt")){ // if marks have been downloaded: parse marks 
                    GetStudentData.PutMarks();
                }
                    
                
                
                //GetStudentData.DownloadLiveMarks(Lines[0],Lines[1], true);
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
            HomeSection.Visibility = Visibility.Visible;
            TasksSection.Visibility = Visibility.Visible;
            WellbeingSection.Visibility = Visibility.Visible;
            GymSection.Visibility = Visibility.Visible;
            MarksSection.Visibility = Visibility.Visible;
        }

        private void MainWinClosed(object sender, EventArgs e)
        {
            
        }
    }
}

