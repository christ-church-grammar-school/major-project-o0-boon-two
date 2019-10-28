using SQLite;
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

        /*public bool UpdateLiveMarksOnOpen = false; //  false for manual true for evey time opened                                                                      
        public bool HomeworkOnPeriodEnd = true;
        public DateTime HomeworkRemind;
        public int RemindForAssignment = 0; //number of days to start reminding before the assignment due date
        public int RemindForTest = 0; //number of days to start reminding before the Test due date*/
        public class Info
        {
            [PrimaryKey, Unique,AutoIncrement]
            public int ID { get; set; }
            public string PersonName { get; set; }
            public string Username { get; set; }

            
            public string Password { get; set; }
            public DateTime LiveMarksUpdate { get; set; }
        }
        public Preferences()
        {
            InitializeComponent();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            
            Info inf = new Info();
            inf.Username = UsernameBox.Text;
            inf.Password = PasswordBox.Password;
                
            LoginStuff.Visibility = Visibility.Collapsed;
            SQLiteConnection conn = new SQLiteConnection("StudentData.sqlite");
            conn.CreateTable<Subject>();
            conn.CreateTable<Info>();
           
            conn.InsertOrReplace(inf);
            conn.CreateTable<Mark>();
            conn.Close();

            var win = new MainWindow();
            win.Show();
            if (sender == subButton)
            {
                GetStudentData.DownloadLiveMarks(true);
                win.ReloadButton.IsEnabled = false;
            }
            this.Close();
        }
    }
}
