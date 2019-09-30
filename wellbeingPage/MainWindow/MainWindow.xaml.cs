using SQLite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static wellbeingPage.Preferences;

namespace wellbeingPage
{
    
    public partial class MainWindow : Window
    {
        

        public bool ShutAll = true;  //when this window shuts everything shuts
        DispatcherTimer milliseconds = new DispatcherTimer();

        public static Info info;


        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new Home();

          
            if (!File.Exists("StudentData.sqlite")) // IF THIS PERSON HAS NOT USED THE APP BEFORE
            {

                Preferences SettingsWin = new Preferences();
                SettingsWin.Show();
                SettingsWin.SettingsFrame.Content = new Login();
                SettingsWin.LoginStuff.Visibility = Visibility.Visible;
                
                ShutAll = false;
                this.Close();
            } else
            {
                GetFromDB();
                if (info.LiveMarksUpdate == "")
                {
                    LastUp.Text = "Last Updated: never";
                } else
                {
                    LastUp.Text = info.LiveMarksUpdate;
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
        public static void GetFromDB() 
        {
            Marks.SubjectResults.Clear();
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");
            
            List<Subject> res = (from m in conn.Table<Subject>() orderby m.YourAverage descending select m).ToList();
            
            info = conn.Table<Info>().ToList()[0];
            
            foreach (var i in res)
            {
                
                var mrks = (from m in conn.Table<Mark>().Where(p => p.subject == i.Name) orderby m.date ascending select m).ToList();
                i.marks.AddRange(mrks);
                Marks.SubjectResults.Add(i);
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
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");
            info.LiveMarksUpdate = LastUp.Text;
            conn.InsertOrReplace(info);
            
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

