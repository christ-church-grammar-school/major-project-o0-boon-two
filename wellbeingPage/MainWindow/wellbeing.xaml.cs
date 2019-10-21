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

            DispatcherTimer audioTimer = new DispatcherTimer();
            audioTimer.Interval = TimeSpan.FromSeconds(1);
            audioTimer.Tick += loadTimer_Tick;
            audioTimer.Start();

            wellbeingPopup.Visibility = Visibility.Visible;
        }

        public void Climbladder()
        {
            ladder(this, EventArgs.Empty);
        }

        /*
        private void addClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Visibility = Visibility.Visible;
            addPopup.Visibility = Visibility.Visible;
        }
        */
       
        private void addCloseClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Visibility = Visibility.Collapsed;
            addPopup.Visibility = Visibility.Collapsed;
        }
      
        private void infoClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Visibility = Visibility.Visible;
            infoPopup.Visibility = Visibility.Visible;
        }

        private void infoCloseClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Visibility = Visibility.Collapsed;
            infoPopup.Visibility = Visibility.Collapsed;
        }

        private void helpClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Visibility = Visibility.Visible;
            helpPopup.Visibility = Visibility.Visible;
        }

        private void helpCloseClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Visibility = Visibility.Collapsed;
            helpPopup.Visibility = Visibility.Collapsed;
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
                mediaPlayer.Play();
            }

            opacityRectangle.Visibility = Visibility.Collapsed;
            addPopup.Visibility = Visibility.Collapsed;

            playButton.IsEnabled = true;
            playButton.Opacity = 1;
            playButton.Visibility = Visibility.Collapsed;
            pauseButton.Visibility = Visibility.Visible;
            mediaLabel.Content = openFileDialog.FileName;
        }

        private void happyClicked(object sender, RoutedEventArgs e)
        {
            wellbeingPopup.Visibility = Visibility.Collapsed;
        }

        void loadTimer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
                if (mediaPlayer.NaturalDuration.HasTimeSpan)
                    audioStatusLabel.Content = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            else
                audioStatusLabel.Content = "0:00 / 0:00";
        }

        private void playClicked(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            playButton.Visibility = Visibility.Collapsed;
            pauseButton.Visibility = Visibility.Visible;
        }

        private void pauseClicked(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            pauseButton.Visibility = Visibility.Collapsed;
            playButton.Visibility = Visibility.Visible;
        }

        private void audio1Clicked(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                mediaPlayer.Stop();
                mediaLabel.Content = "";
                audioStatusLabel.Content = "0:00 / 0:00";
            }

            /*
            mediaPlayer.Open(new Uri());
            */
        }
    }
}
