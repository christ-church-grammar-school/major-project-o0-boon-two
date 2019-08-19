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
using System.Media;

namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for wellbeing.xaml
    /// </summary>
    /// 
    /// I hope everyone can see this?
    public partial class wellbeing : Page
    {
        public EventHandler ladder;

        public wellbeing()
        {
            InitializeComponent();
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

        private void soundBeep(object sender, RoutedEventArgs e)
        {
            SystemSounds.Hand.Play();
        }
    }
}
