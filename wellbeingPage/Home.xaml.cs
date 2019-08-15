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

namespace wellbeingPage
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        DispatcherTimer seconds = new DispatcherTimer();
        DispatcherTimer milliseconds = new DispatcherTimer();

        public Home()
        {
            InitializeComponent();

            seconds.Tick += new EventHandler(OneSecond);
            seconds.Interval = new TimeSpan(0, 0, 1);
            seconds.Start();

            milliseconds.Interval = TimeSpan.FromMilliseconds(1);
            milliseconds.Tick += UpdateSecondHand;
            milliseconds.Start();

        }
        void UpdateSecondHand(object sender, object e)
        {
            secondHand.Angle = (DateTime.Now.Second + (double)DateTime.Now.Millisecond / 1000) * 6;
        }
        private void OneSecond(object sender, EventArgs e)
        {
            minuteHand1.Angle = (DateTime.Now.Minute + (double)DateTime.Now.Second / 60) * 6;
            hourhand1.Angle = (DateTime.Now.Hour + (double)DateTime.Now.Minute / 60) * 15;
        }       
    }
}
