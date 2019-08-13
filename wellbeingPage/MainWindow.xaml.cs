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
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            CloseMenuPopup();

            Timer.Tick += new EventHandler(Timer_Click);

            Timer.Interval = new TimeSpan(0, 0, 1);

            Timer.Start();
        }

        private void CloseMenuPopup()
        {
            DarknessButtonScreen.Margin = new Thickness(10000, 0, 0, 0);
            MenuPopupRectangle.Width = 0;
            TaskPageButton.Width = 0;
            TaskPageTitle.Width = 0;
            TaskPageText.Width = 0;
            WellbeingPageButton.Width = 0;
            WellbeingPageTitle.Width = 0;
            WellbeingPageText.Width = 0;
            GymPageTitle.Width = 0;
            GymPageButton.Width = 0;
            GymPageText.Width = 0;
            LiveMarksPageButton.Width = 0;
            LiveMarksPageTitle.Width = 0;
            LiveMarksPageText.Width = 0;
        }

        private void OpenMenuPopup()
        {
            DarknessButtonScreen.Margin = new Thickness(0, 0, 0, 0);
            MenuPopupRectangle.Width = 1000;
            TaskPageButton.Width = 100;
            TaskPageTitle.Width = 70;
            TaskPageText.Width = 800;
            WellbeingPageButton.Width = 100;
            WellbeingPageTitle.Width = 120;
            WellbeingPageText.Width = 800;
            GymPageButton.Width = 100;
            GymPageTitle.Width = 40;
            GymPageText.Width = 800;
            LiveMarksPageButton.Width = 100;
            LiveMarksPageTitle.Width = 135;
            LiveMarksPageText.Width = 800;
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            TimerTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
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

        }
        private void ShowGymPage()
        {

        }
        private void ShowLiveMarksPage()
        {

        }

        private void DarknessButtonScreenClicked(object sender, RoutedEventArgs e)
        {
            CloseMenuPopup();
        }

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenMenuPopup();
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var yratio = e.NewSize.Height / 764.5;
            var xratio = e.NewSize.Width / 1187;

            DarknessButtonScreen.Width = xratio * 1195;
            DarknessButtonScreen.Height = yratio * 772.5;
        }
    }
}
