using SQLite;
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
using System.Configuration;


namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for Marks.xaml
    /// </summary>
    /// 




    
    //TO DO: create unique identifier including 
    public class Mark
    {
        [Unique, PrimaryKey]
        public string All { get; set; } // year-name-subject
        
        public string subject{ get; set; }
       
        public string name { get; set; }
        public double mark { get; set; }
        public int outOf { get; set; }
        public double weight { get; set; }
        public DateTime date { get; set; }
      
        public string year { get; set; }

        
        public int average { get; set; }
        public double percent { get; set; }

        public string comment { get; set; }
    }
    [Table("Subject")]
    public class Subject
    {
        [Unique,PrimaryKey]
        public string All { get; set; } // year-names

        public string Name { get; set; }
        
        public string Year { get; set; }

        public string teacher { get; set; }
        public int YourAverage { get; set; }
        public int EveryoneAverage { get; set; }
        

        public List<Mark> marks = new List<Mark>();
    }

    public partial class Marks : Page
    {
        public static ObservableCollection<Subject> SubjectResults = new ObservableCollection<Subject>();
        public static ObservableCollection<Subject> CurrentResults = new ObservableCollection<Subject>();
        public static ObservableCollection<string> Years = new ObservableCollection<string>();

        public static string CurrentYear;
        public bool isPopupOpen = false;
        Subject display;
        Mark m;

        public int CurrentSort = 0; //0 - yourav desc 1 - your av asc   2 - classav desx  3 - class av asc    4  - diff desc 5 - diff asc


        public Marks()
        {
            InitializeComponent();
           
            SubjectList.ItemsSource = CurrentResults;
            YearSelect.ItemsSource = Years;
           
            OverallRes.Visibility = Visibility.Visible;
            if (SubjectResults.Count != 0)
            {
                TitleSubject.Content = "Overall";
            }
            
            TitleGrade.Content = "";

            string yr = DateTime.Now.Year.ToString();
            if (!Years.Contains(yr))
            {
                Years.Add(yr);
            }
            
            
            foreach (Subject sub in SubjectResults)
            {
                if (!Years.Contains(sub.Year))
                {
                    Years.Add(sub.Year);
                }
            }


            
            YearSelect.SelectedIndex = 0;
            
            SetCurrentResults();
            UpdateOverallList();
            
        }
      

    void SetCurrentResults()
        {
            CurrentResults.Clear();
            CurrentYear = YearSelect.SelectedValue.ToString().Split(new[] { " " }, StringSplitOptions.None).ToList().Last();

            foreach (Subject sub in SubjectResults)
            {
               
                if (sub.marks.Count >  0 && sub.Year == CurrentYear)
                {
                    CurrentResults.Add(sub);
                }
            }
        }

        private void MyListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem) && !isPopupOpen)
            {
                SubjectList.UnselectAll();
                isPopupOpen = false;
                HoverTestInfo.Visibility = Visibility.Collapsed;
                HoverTestAv.Visibility = Visibility.Collapsed;
                Graph.Children.Clear();
                OverallRes.Visibility = Visibility.Visible;
                TitleSubject.Content = "Overall";
                TitleGrade.Content = "";
            }
            else 
            {
                isPopupOpen = false;
                HoverTestAv.Visibility = Visibility.Collapsed;
                HoverTestInfo.Visibility = Visibility.Collapsed;
                m.comment = MyHoverComments.Text;
            }


            }

        private void changed(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectList.SelectedIndex != -1)
            {
                isPopupOpen = false;
                HoverTestAv.Visibility = Visibility.Collapsed;
                HoverTestInfo.Visibility = Visibility.Collapsed;

                OverallRes.Visibility = Visibility.Collapsed;
                display = CurrentResults[SubjectList.SelectedIndex];
                TitleSubject.Content = display.Name;
                if (display.YourAverage != -1)
                    TitleGrade.Content = display.YourAverage + " %";
                else
                    TitleGrade.Content = "N/A";

                if (display.YourAverage <= 50)
                    TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(181, 0, 0));
                else if (display.YourAverage <= 75)
                    TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(181, Convert.ToByte((181 / 25) * (display.YourAverage - 50)), 0));
                else
                    try
                    {
                        TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(Convert.ToByte(181 - (181 / 25) * (display.YourAverage - 75)), 181, 0));
                    }
                    catch
                    {
                        TitleGrade.Foreground = new SolidColorBrush(Color.FromRgb(0, 181, 0));
                    }

                DrawGraph();
            }
        }

        void UpdateOverallList()
        {
            OverallList.Children.Clear();

            foreach (var sub in CurrentResults)
            {
                AddToOverall(sub.Name, sub.YourAverage, sub.EveryoneAverage);
            }
        }

        void DrawGraph()
        {
            Subject sub = display; 
            Graph.Children.Clear();
            int wi = Convert.ToInt32(Graph.ActualWidth);
            int he = Convert.ToInt32(Graph.ActualHeight);


            if (sub.marks.Count >= 2)
            {
                int incr = wi / (sub.marks.Count - 1);

                PointCollection MyPoints = new PointCollection();
                PointCollection AvPoints = new PointCollection();

                var ButList = new List<Button>();

                var ordered = sub.marks.OrderBy(o => o.date).ToList();

                for (var i = 0; i < sub.marks.Count; i++)
                {
                    Button a = new Button();

                    a.Style = (Style)FindResource("MarkHover");
                    a.Template = (ControlTemplate)FindResource("HMK_Dark");
                    Canvas.SetLeft(a, i * incr - 6.5);
                    Canvas.SetTop(a, he - ordered[i].average * he / 100 - 6.5);
                    a.MouseEnter += GradeHover;
                    a.MouseLeave += GradeStopHover;
                    a.Click += KeepPopup;
                    a.Name = "Av" + i;
                    ButList.Add(a);

                    Button b = new Button();

                    b.Style = (Style)FindResource("MarkHover");
                    b.Template = (ControlTemplate)FindResource("HMK");
                    Canvas.SetLeft(b, i * incr - 6.5);
                    Canvas.SetTop(b, he - ordered[i].percent * he/ 100 - 6.5);
                    b.MouseEnter += GradeHover;
                    b.MouseLeave += GradeStopHover;
                    b.Click += KeepPopup;
                    b.Name = "My" + i;
                    ButList.Add(b);




                    MyPoints.Add(new Point(i * incr, he - ordered[i].percent * he / 100));
                    AvPoints.Add(new Point(i * incr, he - ordered[i].average * he / 100));
                }
                Polyline MyLline = new Polyline
                {
                    StrokeThickness = 3,
                    Stroke = Brushes.Red,
                    Points = MyPoints
                };
                Polyline AvLline = new Polyline
                {
                    StrokeThickness = 3,
                    Stroke = Brushes.DarkGray,
                    Points = AvPoints
                };
                Graph.Children.Add(AvLline);
                Graph.Children.Add(MyLline);
                foreach (var i in ButList)
                {
                    Graph.Children.Add(i);
                }

            }
            else if (sub.marks.Count() == 1)
            {
                Button a = new Button();

                a.Style = (Style)FindResource("MarkHover");
                a.Template = (ControlTemplate)FindResource("HMK_Dark");
                Canvas.SetLeft(a,-6.5);
                Canvas.SetTop(a, he - sub.marks[0].average * he / 100 - 6.5);
                a.MouseEnter += GradeHover;
                a.MouseLeave += GradeStopHover;
                a.Click += KeepPopup;
                a.Name = "Av" + 0;
                Graph.Children.Add(a);

                Button b = new Button();

                b.Style = (Style)FindResource("MarkHover");
                b.Template = (ControlTemplate)FindResource("HMK");
                Canvas.SetLeft(b, - 6.5);
                Canvas.SetTop(b, he - sub.marks[0].percent * he / 100 - 6.5);
                b.MouseEnter += GradeHover;
                b.MouseLeave += GradeStopHover;
                b.Click += KeepPopup;
                b.Name = "My" + 0;
                Graph.Children.Add(b);
            }

        }
        private void SizeChangedd(object sender, EventArgs e)
        {
            if (SubjectList.SelectedIndex != -1)
            {
                DrawGraph();
            }
            HoverTestAv.Visibility = Visibility.Collapsed;
            HoverTestInfo.Visibility = Visibility.Collapsed;
            isPopupOpen = false;
        }

        private void GradeHover(object sender, MouseEventArgs e)
        {
            try
            {
                string name = ((Button)sender).Name;
                Subject sub = CurrentResults[SubjectList.SelectedIndex];


                var val = Convert.ToInt32(name[2]) - 48;
                System.Windows.Point pos = e.GetPosition(DataBackround);

                var x = pos.X +  10;
                var y = pos.Y + 10;

                if (x > DataBackround.ActualWidth / 2)
                {

                    x -= HoverTestInfo.Width + 20;
                }


                m = sub.marks[val];

                if (name[0] == 'M') // It is persons grade
                {
                    if (y > DataBackround.ActualHeight / 2)
                    {
                        y -= HoverTestInfo.Height + 20;
                    }
                    Thickness margin = new Thickness(x, y, 0, 0);

                    MyHoverDate.Text = "Date: " + m.date.ToShortDateString();
                    MyHoverMarks.Text = "Mark: " + m.mark + " / " + m.outOf;
                    MyHoverP.Text = "" + m.percent;
                    MyHoverComments.Text = m.comment;
                    MyHoverWeight.Text = "Weight: " + m.weight;
                    MyHoverName.Text = m.name;
                    HoverTestInfo.Margin = margin;


                    HoverTestInfo.Visibility = Visibility.Visible;
                    HoverTestAv.Visibility = Visibility.Collapsed;
                    isPopupOpen = false;
                }
                else // average grade
                {
                    if (y > DataBackround.ActualHeight / 2)
                    {
                        y -= HoverTestAv.Height +20;
                    }
                    Thickness margin = new Thickness(x, y, 0, 0);

                    AvHoverDate.Text = "Date: " + m.date.ToShortDateString();

                    AvHoverMark.Text = "Mark: " + Convert.ToString(Math.Round( (double)m.outOf*m.average/100,1)) + " / " + m.outOf;
                    AvHoverP.Text = "" + m.average;
                    AvHoverWeight.Text = "Weight: " + m.weight;
                    AvHoverName.Text = m.name;
                    HoverTestAv.Margin = margin;
                    HoverTestAv.Visibility = Visibility.Visible;
                    HoverTestInfo.Visibility = Visibility.Collapsed;
                    isPopupOpen = false;
                }

            }
            catch
            {
                SubjectList.UnselectAll();
                isPopupOpen = false;
                HoverTestInfo.Visibility = Visibility.Collapsed;
                HoverTestAv.Visibility = Visibility.Collapsed;
                Graph.Children.Clear();
                OverallRes.Visibility = Visibility.Visible;
                TitleSubject.Content = "Overall";
                TitleGrade.Content = "";
            }
        }

        private void GradeStopHover(object sender, MouseEventArgs e)
        {
            if (!isPopupOpen)
            {
                try
                {
                    HoverTestAv.Visibility = Visibility.Collapsed;
                    HoverTestInfo.Visibility = Visibility.Collapsed;
                    m.comment = MyHoverComments.Text;
                }
                catch { }
            }
            
        }

        private void KeepPopup(object sender, RoutedEventArgs e)
        {
            isPopupOpen = !isPopupOpen;
        }

        private void YearChanged(object sender, SelectionChangedEventArgs e)
        {
            SubjectList.UnselectAll();
            isPopupOpen = false;
            HoverTestInfo.Visibility = Visibility.Collapsed;
            HoverTestAv.Visibility = Visibility.Collapsed;
            Graph.Children.Clear();
            OverallRes.Visibility = Visibility.Visible;
            TitleSubject.Content = "Overall";
            TitleGrade.Content = "";

            
            SetCurrentResults();

            OverallList.Children.Clear();

            foreach (var sub in CurrentResults)
            {
                AddToOverall(sub.Name, sub.YourAverage, sub.EveryoneAverage);
            }
        }

       
        private void AddToOverall(string name, int yourAv, int ClassAv)
        {
            var grid = new Grid() {
                Height = 196,
                Width = 128
            };
            var SubName = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Text = name,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = ResultsLabel.FontFamily,
                Foreground = Brushes.White,
                Height = 66,
                TextAlignment = TextAlignment.Center,
                FontSize = 16
            };
            var SubAv = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Text = yourAv.ToString(),
                FontFamily = ResultsLabel.FontFamily,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0,66,0,0),
                Height = 66,
                TextAlignment = TextAlignment.Center,
                FontSize = 24,
                
            };

            if (yourAv <= 50)
                SubAv.Foreground = new SolidColorBrush(Color.FromRgb(181, 0, 0));
            else if (yourAv <= 75)
                SubAv.Foreground = new SolidColorBrush(Color.FromRgb(181, Convert.ToByte((181 / 25) * (yourAv - 50)), 0));
            else
                try
                {
                    SubAv.Foreground = new SolidColorBrush(Color.FromRgb(Convert.ToByte(181 - (181 / 25) * (yourAv - 75)), 181, 0));
                }
                catch
                {
                    SubAv.Foreground = new SolidColorBrush(Color.FromRgb(0, 181, 0));
                }
            

            var ClassAvText = new TextBlock()
            {
               
                TextWrapping = TextWrapping.Wrap,
                Text = ClassAv.ToString(),
                FontFamily = ResultsLabel.FontFamily,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = new SolidColorBrush(Color.FromRgb(Convert.ToByte(55+2*ClassAv), Convert.ToByte(55 + 2*ClassAv), Convert.ToByte(55 + 2*ClassAv))),
                Height = 66,
                Margin = new Thickness(0, 110, 0, 0),
                TextAlignment = TextAlignment.Center,
                FontSize = 24
            };
            var Dif = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Text = (yourAv - ClassAv).ToString(),
                FontFamily = ResultsLabel.FontFamily,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = Brushes.White,
                Height = 66,
                Margin = new Thickness(0, 154, 0, 0),
                TextAlignment = TextAlignment.Center,
                FontSize = 24
            };
            grid.Children.Add(SubName);
            grid.Children.Add(SubAv);
            grid.Children.Add(ClassAvText);
            grid.Children.Add(Dif);

            OverallList.Children.Add(grid);
        }

        private void AverageSort(object sender, RoutedEventArgs e)
        {
            if (CurrentSort == 0)
            {
                var a = CurrentResults.OrderBy(o => o.YourAverage).ToList();
                CurrentSort = 1;
                CurrentResults.Clear();
                foreach (var b in a)
                {
                    CurrentResults.Add(b);
                }
            }
            else
            {
                var a = CurrentResults.OrderByDescending(o => o.YourAverage).ToList();
                CurrentSort = 0;
                CurrentResults.Clear();
                foreach (var b in a)
                {
                    CurrentResults.Add(b);
                }
            }

            UpdateOverallList();
            overallScroll.ScrollToLeftEnd();
        }

        private void ClassSort(object sender, RoutedEventArgs e)
        {
            if (CurrentSort == 2)
            {
                var a = CurrentResults.OrderBy(o => o.EveryoneAverage).ToList();
                CurrentSort = 3;
                CurrentResults.Clear();
                foreach (var b in a)
                {
                    CurrentResults.Add(b);
                }
            }
            else
            {
                var a = CurrentResults.OrderByDescending(o => o.EveryoneAverage).ToList();
                CurrentSort = 2;
                CurrentResults.Clear();
                foreach (var b in a)
                {
                    CurrentResults.Add(b);
                }
            }

            UpdateOverallList();
            overallScroll.ScrollToLeftEnd();
        }

        private void DifferenceSort(object sender, RoutedEventArgs e)
        {
            if (CurrentSort == 4)
            {
                var a = CurrentResults.OrderBy(o => (o.YourAverage - o.EveryoneAverage)).ToList();
                CurrentSort = 5;
                CurrentResults.Clear();
                foreach (var b in a)
                {
                    CurrentResults.Add(b);
                }
            }
            else
            {
                var a = CurrentResults.OrderByDescending(o => (o.YourAverage - o.EveryoneAverage)).ToList();
                CurrentSort = 4;
                CurrentResults.Clear();
                foreach (var b in a)
                {
                    CurrentResults.Add(b);
                }
            }
            
            UpdateOverallList();
            overallScroll.ScrollToLeftEnd();
        }
    }
}