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
using Microsoft.Win32;
using System.Media;

namespace wellbeingPage
{
    public partial class wellbeing : Page
    {
        public EventHandler ladder;
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public wellbeing()
        {
            InitializeComponent();

            if (DateTime.Now.Hour > 0)
            {
                greetingLabel.Content = "Good Morning,";
            }

            if (DateTime.Now.Hour > 12)
            {
                greetingLabel.Content = "Good Afternoon,";
            }

            if (DateTime.Now.Hour > 17.5)
            {
                greetingLabel.Content = "Good Evening,";
            }

            else
            {
                greetingLabel.Content = "Hello,";
            }
        }

        public void Climbladder()
        {
            ladder(this, EventArgs.Empty);
        }

        private void addClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Width = 1200;
            addPopup.Visibility = Visibility.Visible;
        }

        private void addCloseClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Width = 0;
            addPopup.Visibility = Visibility.Collapsed;
        }

        private void infoClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Width = 1200;
            infoPopup.Visibility = Visibility.Visible;
        }

        private void infoCloseClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Width = 0;
            infoPopup.Visibility = Visibility.Collapsed;
        }

        private void menuButtonClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Visible;
        }

        private void DarknessButtonScreenClicked(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
        }

        private void audioClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
            }
        }
    }
}
