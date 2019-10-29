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
                if (info.LiveMarksUpdate == "" || info.LiveMarksUpdate == "Last Updated: N/A")
                {
                    LastUp.Text = "Last Updated: N/A";
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
                Year = Marks.CurrentYear, 
                
            });
            AddMarksDisabledRec.Visibility = Visibility.Collapsed;
            AddMarks.IsEnabled = true;
       
        }

        private void AddMrk(object sender, RoutedEventArgs e)
        {
           
            Marks.CurrentResults[SubjectList.SelectedIndex].marks.Add(new Mark() {
                name = "Untitled\n",
                
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
            string Name = ((Button)sender).Tag.ToString();
           

            var selected = MessageBox.Show("Are you sure you would like to delete " + Name, "Delete Mark", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (selected == MessageBoxResult.OK)
            {
                // Find subject to delete in CurrentResults
                for (int i = 0; i < Marks.CurrentResults[SubjectList.SelectedIndex].marks.Count(); i++)
                {
                    if (Marks.CurrentResults[SubjectList.SelectedIndex].marks[i].name == Name)
                    {
                        Marks.CurrentResults[SubjectList.SelectedIndex].marks.RemoveAt(i);
                    }
                }

                int index = 0;
                for(var a = 0; a < Marks.SubjectResults.Count(); a ++)
                {
                    if (Marks.SubjectResults[a].All == Marks.CurrentResults[SubjectList.SelectedIndex].All)
                    {
                        index = a;
                    }
                }
                // Find subject to delete in SubjectResults
                for (int i = 0; i < Marks.SubjectResults[index].marks.Count(); i++)
                {
                    if (Marks.SubjectResults[index].marks[i].name == Name)
                    {
                        Marks.SubjectResults[index].marks.RemoveAt(i);
                    }
                }

                SQLiteConnection con = new SQLiteConnection("StudentData.sqlite");


                con.Execute("DELETE FROM Mark WHERE [subject] = '" + Marks.CurrentResults[SubjectList.SelectedIndex].Name + "' and [year] = '" + Marks.CurrentYear + "' and [name] = '" + Name + "'");

                int sel = MarksList.SelectedIndex;

                MarksList.ItemsSource = null;                                                             //update source with new item
                MarksList.ItemsSource = Marks.CurrentResults[SubjectList.SelectedIndex].marks;

                if (MarksList.SelectedIndex == -1)
                {
                    try
                    {
                        MarksList.SelectedIndex = 0;
                        MarksList.ScrollIntoView(MarksList.Items[0]);

                        MarksList.SelectedIndex = sel;
                        MarksList.ScrollIntoView(MarksList.Items[sel]);
                    }
                    catch { }
                    
                }
            }
        }

        private void DeleteSub(object sender, RoutedEventArgs e)
        {
           string Name = ((Button)sender).Tag.ToString();
            string all = Marks.CurrentYear+Name;
            var a = new List<Mark>() { };

           var selected = MessageBox.Show("Are you sure you would like to delete " + Name, "Delete Subject", MessageBoxButton.OKCancel, MessageBoxImage.Question);
           if (selected == MessageBoxResult.OK)
            {
                // Find subject to delete in SubjectResults
                for (int i= 0; i < Marks.SubjectResults.Count(); i++)
                {
                    if (Marks.SubjectResults[i].All == all)
                    {
                        Marks.SubjectResults.RemoveAt(i);
                    }
                }

                // Find subject to delete in CurrentResults
                for (int i = 0; i < Marks.CurrentResults.Count(); i++)
                {
                    if (Marks.CurrentResults[i].Name == Name)
                    {
                        Marks.CurrentResults.RemoveAt(i);
                    }
                }

                SQLiteConnection con = new SQLiteConnection("StudentData.sqlite");
                
                
                con.Execute("DELETE FROM Subject WHERE [All] = '" + all + "'");
                con.Execute("DELETE FROM Mark WHERE [subject] = '" + Name + "' and [year] = '"+ Marks.CurrentYear + "'");
                if (SubjectList.SelectedIndex == -1)
                {
                    try
                    {
                        SubjectList.SelectedIndex = 0;
                        SubjectList.ScrollIntoView(SubjectList.Items[0]);
                    } catch
                    {
                        MarksList.ItemsSource = a;

                    }
                    
                }
            }

        }
    }
}


