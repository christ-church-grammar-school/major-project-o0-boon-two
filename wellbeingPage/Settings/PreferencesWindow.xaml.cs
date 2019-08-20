using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using wellbeingPage.Settings;

namespace wellbeingPage
{

    
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {

        public bool UpdateLiveMarksOnOpen = false; //  false for manual true for evey time opened                                                                      
        public bool HomeworkOnPeriodEnd = true;
        public DateTime HomeworkRemind;
        public int RemindForAssignment= 0; //number of days to start reminding before the assignment due date
        public int RemindForTest = 0; //number of days to start reminding before the Test due date

        public Preferences()
        {
            InitializeComponent();
            MainFrame.Content = new SetSettings();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory("data");
            using (StreamWriter outputFile = new StreamWriter("data/cred.txt"))
            {
                outputFile.WriteLine(UsernameBox.Text);
                outputFile.WriteLine(PasswordBox.Password.ToString());
            }

            MainWindow win = new MainWindow();
            win.Show();
            LoginStuff.Visibility = Visibility.Collapsed;
            this.Close();
        }
    }
}
