﻿using SQLite;
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
using static wellbeingPage.Preferences;

namespace wellbeingPage
{
    
    public partial class MainWindow : Window
    {
        

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
               
                this.Hide();
            } else
            {
                GetFromDB();
                if (info.LiveMarksUpdate == "" || info.LiveMarksUpdate == "Last Updated: never")
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

            SubjectList.ItemsSource = Marks.CurrentResults;
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
                
                var mrks = (from m in conn.Table<Mark>().Where(p => p.subject == i.Name && p.year == i.Year) orderby m.date ascending select m).ToList();
                
                
                i.marks.AddRange(mrks);
                Marks.SubjectResults.Add(i);
            }

            


        }
        private void DarknessButtonScreenClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
            EditMarks.Visibility = Visibility.Collapsed;
            SettingsPopup.Visibility = Visibility.Collapsed;
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
            EditButton.Visibility = Visibility.Visible;
           
            MainFrame.Content = new Marks();
        }
        private void HomeClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
            ShowAllPages();
            HomeSection.Visibility = Visibility.Collapsed;
            MainFrame.Content = new Home();
        }
        void ShowAllPages() // on the menu popup options
        {
            LastUp.Visibility = Visibility.Collapsed;
            ReloadButton.Visibility = Visibility.Collapsed;
            ReloadRotater.Angle = 0;
            HomeSection.Visibility = Visibility.Visible;
            TasksSection.Visibility = Visibility.Visible;
            WellbeingSection.Visibility = Visibility.Visible;
            GymSection.Visibility = Visibility.Visible;
            MarksSection.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Collapsed;
        }

        private void MainWinClosed(object sender, EventArgs e)
        {
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");
            try
            {
                info.LiveMarksUpdate = LastUp.Text;
                conn.InsertOrReplace(info);
            }
            catch { }

            Application.Current.Shutdown();



        }
        private void ReloadMarks(object sender, RoutedEventArgs e)
        {
            GetStudentData.DownloadLiveMarks(true);
            ReloadButton.IsEnabled = false;
        }

        private void CreateEditMarksPopup(object sender, RoutedEventArgs e)
        {
            
            if (Marks.CurrentResults.Count == 0)
            {
                AddMarks.IsEnabled = false;
                AddMarksDisabledRec.Visibility = Visibility.Visible;
            }
            EditMarks.Visibility = Visibility.Visible;
            try
            {
                SubjectList.SelectedIndex = 0;
            }
            catch
            {

            }
        }

        private void SubjectChanged(object sender, RoutedEventArgs e)
        {
   
           
            try
            {
                MarksList.ItemsSource = Marks.CurrentResults[SubjectList.SelectedIndex].marks;
                MarksList.ScrollIntoView(MarksList.Items[0]);
            }
            catch { }
            try
            {
                MarksList.SelectedIndex = 0;
                MarksList.ScrollIntoView(MarksList.Items[0]);
            }
            catch { }
            
        }

        private void AddSub(object sender, RoutedEventArgs e)
        {
            Marks.CurrentResults.Add(new Subject()
            {
                Name = "Untitled\n",
                Year = DateTime.Now.Year.ToString(), // not necessarily..
                
            });
            AddMarksDisabledRec.Visibility = Visibility.Collapsed;
            AddMarks.IsEnabled = true;
       
        }

        private void AddMrk(object sender, RoutedEventArgs e)
        {
           
            Marks.CurrentResults[SubjectList.SelectedIndex].marks.Add(new Mark() {
                name = "untitled\n",
                
            });

            MarksList.ItemsSource = null;                                                             //update source with new item
            MarksList.ItemsSource = Marks.CurrentResults[SubjectList.SelectedIndex].marks;
            MarksList.SelectedIndex = MarksList.Items.Count - 1;
            // MarksList.ItemsSource = Marks.CurrentResults[SubjectList.SelectedIndex].marks;
        }

        private void MarkChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Mark mrk = Marks.CurrentResults[SubjectList.SelectedIndex].marks[MarksList.SelectedIndex];
                Subject sub = Marks.CurrentResults[SubjectList.SelectedIndex];

                SubName.Text = sub.Name;
                SubYear.Text = sub.Year;

                MarkName.Text = mrk.name;
                MarkDate.Text = mrk.date.ToString();
                MarkMark.Text = mrk.mark;
            }
            catch { }
        }

        private void Save(object sender, RoutedEventArgs e)
        {

        }

        private void SubYear_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DeleteMark(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSub(object sender, RoutedEventArgs e)
        {
            //var sub = VisualTreeHelper.GetParent(sender as Button);
        }
    }
}