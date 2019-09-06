﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml;


namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for Marks.xaml
    /// </summary>
    public class Mark
    {
        public string name;
        public string mark;
        public double weight;
        public string date;
        public int average;

        public string comment;
    }
    [Table("Subject")]
    public class Subject
    {
        [Unique]
        public string Name { get; set; }

        public string teacher { get; set; }
        public int YourAverage { get; set; }
        public int EveryoneAverage { get; set; }

        public int SubjectID;

        public List<Mark> marks = new List<Mark>();
    }

    public partial class Marks : Page
    {

            
        public static ObservableCollection<Subject> SubjectResults = new ObservableCollection<Subject>();
        
        public Marks()
        {
            InitializeComponent();
           

            OverallRes.Visibility = Visibility.Visible;
            if (SubjectResults.Count != 0)
            {
                TitleSubject.Content = "Overall";
            }
                        
            TitleGrade.Content = "";
            
            
            SubjectList.ItemsSource = SubjectResults;
            OverallRes.ItemsSource = SubjectResults;

            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");

            conn.CreateTable<Subject>();
            foreach(var i in SubjectResults)
                try
                {
                    conn.Insert(i);
                }
                catch
                {

                }
            
            
        }

        private void MyListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                SubjectList.UnselectAll();
            OverallRes.Visibility = Visibility.Visible;
            TitleSubject.Content = "Overall";
            TitleGrade.Content = "";
            

        }

        private void changed(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectList.SelectedIndex != -1)
            {
                OverallRes.Visibility = Visibility.Collapsed;
                var display = SubjectResults[SubjectList.SelectedIndex];
                TitleSubject.Content = display.Name;
                if (display.YourAverage != -1)
                    TitleGrade.Content = display.YourAverage + " %";
                else
                    TitleGrade.Content = "N/A";

                if (display.YourAverage <= 50)
                    TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(181,0,0));
                else if (display.YourAverage <= 75)
                    TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(181, Convert.ToByte((181/25)*(display.YourAverage-50)), 0));
                else
                    TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(Convert.ToByte(181 - (181 / 25) * (display.YourAverage - 75)), 181, 0));
            }
            else
            {
                
            }
          
        }
        
        
    }
}