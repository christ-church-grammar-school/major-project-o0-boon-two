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
    /// Interaction logic for wellbeing.xaml
    /// </summary>
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
            addPopupBottom.Width = 1000;
            addPopupTop.Width = 1000;
            addCloseButton.Width = 80;
        }

        private void addCloseClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Width = 0;
            addPopupBottom.Width = 0;
            addPopupTop.Width = 0;
            addCloseButton.Width = 0;
        }

        private void infoClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Width = 1200;
            infoPopupBottom.Width = 1000;
            infoPopupTop.Width = 1000;
            infoCloseButton.Width = 80;
        }

        private void infoCloseClicked(object sender, RoutedEventArgs e)
        {
            opacityRectangle.Width = 0;
            infoPopupBottom.Width = 0;
            infoPopupTop.Width = 0;
            infoCloseButton.Width = 0;
        }
    }
}
